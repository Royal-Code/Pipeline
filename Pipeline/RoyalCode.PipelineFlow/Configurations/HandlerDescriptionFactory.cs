using RoyalCode.PipelineFlow.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal static class HandlerDescriptionFactory
    {
        internal static HandlerDescription GetHandlerDescription(this Delegate handler)
        {
            var method = handler.Method;
            var parms = method.GetParameters();

            // first check parameters
            if (parms.Length is not 1 and not 2)
                throw new InvalidHandlerDelegateException();

            // the input type.
            var inputType = parms[0].ParameterType;

            // the output type
            var outputType = method.ReturnType;
            bool hasOutput = true;
            bool isAsync = false;
            bool hasToken = false;

            if (outputType == typeof(void))
            {
                hasOutput = false;
            }
            else if (outputType == typeof(Task))
            {
                isAsync = true;

                // check if a Task<>, with result.
                if (outputType.IsGenericType)
                {
                    // get the result type.
                    outputType = outputType.GetGenericArguments()[0];
                }
                else
                {
                    // Task without result is like void.
                    hasOutput = false;
                }
            }

            // check parameters
            if (isAsync)
            {
                if (parms.Length is 2)
                {
                    if (parms[1].ParameterType != typeof(CancellationToken))
                    {
                        throw new InvalidHandlerDelegateException();
                    }
                    else
                    {
                        hasToken = true;
                    }
                }
            }
            else if (parms.Length != 1)
            {
                throw new InvalidHandlerDelegateException();
            }

            var description = new HandlerDescription()
            {
                InputType = inputType,
                OutputType = outputType,
                HasOutput = hasOutput,
                IsAsync = isAsync,
                HasToken = hasToken,
                IsBridge = false,
                HandlerDelegateProvider = (_, _) => handler
            };

            return description;
        }

        internal static HandlerDescription GetHandlerDescription(this MethodInfo method)
        {
            var serviceType = method.DeclaringType;

            var parms = method.GetParameters();

            // first check parameters
            if (parms.Length is not 1 and not 2)
                throw new InvalidHandlerMethodException();

            // the input type.
            var inputType = parms[0].ParameterType;

            // the output type
            var outputType = method.ReturnType;
            bool hasOutput = true;
            bool isAsync = false;
            bool hasToken = false;

            if (outputType == typeof(void))
            {
                hasOutput = false;
            }
            else if (outputType == typeof(Task))
            {
                isAsync = true;

                // check if a Task<>, with result.
                if (outputType.IsGenericType)
                {
                    // get the result type.
                    outputType = outputType.GetGenericArguments()[0];
                }
                else
                {
                    // Task without result is like void.
                    hasOutput = false;
                }
            }

            // check parameters
            if (isAsync)
            {
                if (parms.Length is 2)
                {
                    if (parms[1].ParameterType != typeof(CancellationToken))
                    {
                        throw new InvalidHandlerMethodException();
                    }
                    else
                    {
                        hasToken = true;
                    }
                }
            }
            else if (parms.Length != 1)
            {
                throw new InvalidHandlerMethodException();
            }

            // create a delegate

            ParameterExpression serviceParam, inputParam, tokenParam;
            List<ParameterExpression> parameters = new();

            serviceParam = Expression.Parameter(serviceType, "service");
            inputParam = Expression.Parameter(inputType, "input");
            parameters.Add(serviceParam);
            parameters.Add(inputParam);

            if (hasToken)
            {
                tokenParam = Expression.Parameter(typeof(CancellationToken), "token");
                parameters.Add(tokenParam);
            }

            // define the delegate type
            Type delegateType;
            if (hasOutput || isAsync)
            {
                if (hasToken)
                {
                    delegateType = typeof(Func<,,,>)
                        .MakeGenericType(serviceType, inputType, typeof(CancellationToken), method.ReturnType);
                }
                else
                {
                    delegateType = typeof(Func<,,>)
                        .MakeGenericType(serviceType, inputType, method.ReturnType);
                }
            }
            else
            {
                delegateType = typeof(Action<,>).MakeGenericType(serviceType, inputType);
            }

            var body = Expression.Call(serviceParam, method, parameters.Skip(1));
            var lambda = Expression.Lambda(delegateType, body, parameters);
            var methodDelegate = lambda.Compile();

            var description = new HandlerDescription()
            {
                InputType = inputType,
                OutputType = outputType,
                HasOutput = hasOutput,
                IsAsync = isAsync,
                HasToken = hasToken,
                IsBridge = false,
                //HandlerDelegate = methodDelegate, TODO -> usar GenericResolution
                ServiceType = serviceType
            };

            return description;
        }

        internal static HandlerDescription GetHandlerDescription(this Delegate handler, Type serviceType)
        {
            var method = handler.Method;
            var parms = method.GetParameters();

            // first check parameters
            if (parms.Length is not 2 and not 3)
                throw new InvalidServiceHandlerDelegateException();

            // check that first param must be the service
            if (parms[0].ParameterType != serviceType)
            {
                throw new InvalidServiceHandlerDelegateException();
            }

            // the input type.
            var inputType = parms[1].ParameterType;
            
            // the output type
            var outputType = method.ReturnType;
            bool hasOutput = true;
            bool isAsync = false;
            bool hasToken = false;

            if (outputType == typeof(void))
            {
                hasOutput = false;
            }
            else if (outputType == typeof(Task))
            {
                isAsync = true;

                // check if a Task<>, with result.
                if (outputType.IsGenericType)
                {
                    // get the result type.
                    outputType = outputType.GetGenericArguments()[0];
                }
                else
                {
                    // Task without result is like void.
                    hasOutput = false;
                }
            }

            // check parameters
            if (isAsync)
            {
                if (parms.Length is 3)
                {
                    if (parms[2].ParameterType != typeof(CancellationToken))
                    {
                        throw new InvalidServiceHandlerDelegateException();
                    }
                    else
                    {
                        hasToken = true;
                    }
                }
            }
            else if (parms.Length != 2)
            {
                throw new InvalidServiceHandlerDelegateException();
            }

            var description = new HandlerDescription()
            {
                InputType = inputType,
                OutputType = outputType,
                HasOutput = hasOutput,
                IsAsync = isAsync,
                HasToken = hasToken,
                IsBridge = false,
                HandlerDelegateProvider = (_, _) => handler,
                ServiceType = serviceType
            };

            return description;
        }
    }

    /// <summary>
    /// Contém a resolução dos tipos genéricos do input e output em relação ao serviço de um method handler.
    /// </summary>
    public class GenericResolution
    {
        // deve conter uma relação/mapeamento dos parametros genéricos do serviço em relação
        // aos parâmetros genéricos do input e em relação ao output ou parâmetros genéricos do output.

        private readonly Type inputType;
        private readonly Type outputType;
        private readonly bool isAsync;
        private readonly bool hasToken;
        private readonly MemberInfo handlerMethod;

        public GenericResolution(Type inputType, Type outputType, bool isAsync, bool hasToken, MemberInfo handlerMethod)
        {
            this.inputType = inputType ?? throw new ArgumentNullException(nameof(inputType));
            this.outputType = outputType ?? throw new ArgumentNullException(nameof(outputType));
            this.isAsync = isAsync;
            this.hasToken = hasToken;
            this.handlerMethod = handlerMethod ?? throw new ArgumentNullException(nameof(handlerMethod));
        }

        public Delegate CreateDelegate(Type inputType, Type outputType)
        {
            // resolver o typo do serviço a partir do tipo do input e output.
            // resolver o método real.
            // criar o delegate.

            throw new NotImplementedException();
        }
    }
}
