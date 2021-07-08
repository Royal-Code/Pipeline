using RoyalCode.PipelineFlow.Exceptions;
using System;
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

            var resolution = new GenericResolution(inputType, outputType, isAsync, hasOutput, method);
            
            var description = new HandlerDescription()
            {
                InputType = inputType,
                OutputType = outputType,
                HasOutput = hasOutput,
                IsAsync = isAsync,
                HasToken = hasToken,
                IsBridge = false,
                HandlerDelegateProvider = (i, o) => resolution.CreateDelegate(i, o),
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
}
