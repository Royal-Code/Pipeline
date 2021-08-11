﻿using RoyalCode.PipelineFlow.Exceptions;
using RoyalCode.PipelineFlow.Extensions;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal class DescriptionBuilder
    {
        private ChainKind kind;
        private readonly Delegate? handler;
        private readonly MethodInfo method;
        private readonly ParameterInfo[] parms;
        private readonly bool handlerHasService;
        private readonly Type? serviceType;

        private Type? inputType;
        private OutputDescription? output;
        private bool hasToken;
        private GenericResolution? resolution;
        private BridgeNextHandlerDescription? bridgeNextHandlerDescription;

        private DescriptionBuilder(Delegate handler)
        {
            this.handler = handler;
            method = handler.Method;
            parms = method.GetParameters();
            handlerHasService = false;
        }

        private DescriptionBuilder(Delegate handler, Type serviceType)
        {
            this.handler = handler;
            this.serviceType = serviceType;
            method = handler.Method;
            parms = method.GetParameters();
            handlerHasService = true;
        }

        private DescriptionBuilder(MethodInfo method)
        {
            serviceType = method.DeclaringType;
            handlerHasService = false;
            this.method = method;
            parms = method.GetParameters();
            handler = null;
        }

        public static DescriptionBuilder Create(Delegate handler) => new(handler);

        public static DescriptionBuilder Create(Delegate handler, Type serviceType) => new(handler, serviceType);

        public static DescriptionBuilder Create(MethodInfo method) => new(method);

        #region Handler Functions

        public void ReadHandlerParameters()
        {
            // first check parameters
            if (handlerHasService)
            {
                if (parms.Length is not 2 and not 3)
                    throw new InvalidHandlerDelegateException();

                // check that first param must be the service
                if (!parms[0].ParameterType.Implements(serviceType!))
                {
                    throw new InvalidServiceHandlerDelegateException();
                }
            }
            else
            {
                if (parms.Length is not 1 and not 2)
                    throw new InvalidHandlerDelegateException();
            }

            // the input type.
            inputType = parms[handlerHasService ? 1 : 0].ParameterType;

            kind = ChainKind.Handler;

            output = new OutputDescription(method);
        }

        public void ValidateHandlerParameters()
        {
            if (output is null)
                throw new InvalidOperationException("Read handler parameters is required.");

            // check parameters
            if (output.IsAsync)
            {
                if (parms.Length == (handlerHasService ? 3 : 2))
                {
                    if (parms[handlerHasService ? 2 : 1].ParameterType != typeof(CancellationToken))
                    {
                        throw new InvalidHandlerDelegateException();
                    }
                    else
                    {
                        hasToken = true;
                    }
                }
            }
            else if (parms.Length != (handlerHasService ? 2 : 1))
            {
                throw new InvalidHandlerDelegateException();
            }
        }

        public HandlerDescription BuildHandlerDescription()
        {
            if (kind is not ChainKind.Handler)
                throw new InvalidOperationException("The builder is not for handler.");

            if (output is null || inputType is null)
                throw new InvalidOperationException("Read decorator parameters is required.");

            Func<Type, Type, Delegate> provider;
            if (handler is not null)
                provider = (_, _) => handler;
            else if (resolution is not null)
                provider = resolution.CreateDelegate;
            else
                throw new InvalidOperationException("Handler provider can't be resolved");

            return new HandlerDescription(inputType, output.OutputType, provider)
            {
                HasToken = hasToken,
                HasOutput = output.HasOutput,
                IsAsync = output.IsAsync
            };
        }

        #endregion

        #region Decorator Functions

        public void ReadDecoratorParameters()
        {
            // first check parameters
            if (handlerHasService)
            {
                if (parms.Length is not 3 and not 4)
                    throw new InvalidDecoratorDelegateException();

                // check that first param must be the service
                if (parms[0].ParameterType.Implements(serviceType!))
                {
                    throw new InvalidServiceHandlerDelegateException();
                }
            }
            else
            {
                if (parms.Length is not 2 and not 3)
                    throw new InvalidDecoratorDelegateException();
            }

            // the input type.
            inputType = parms[handlerHasService ? 1 : 0].ParameterType;

            kind = ChainKind.Decorator;

            output = new OutputDescription(method);
        }

        public void ValidateDecoratorParameters()
        {
            if (output is null)
                throw new InvalidOperationException("Read decorator parameters is required.");

            // check next handler
            var nextType = output.IsVoid
                ? typeof(Action)
                : typeof(Func<>).MakeGenericType(method.ReturnType);

            if (parms[handlerHasService ? 2 : 1].ParameterType != nextType)
                throw new InvalidDecoratorDelegateException();

            // check parameters
            if (output.IsAsync)
            {
                if (parms.Length == (handlerHasService ? 4 : 3))
                {
                    if (parms[handlerHasService ? 3 : 2].ParameterType != typeof(CancellationToken))
                    {
                        throw new InvalidDecoratorDelegateException();
                    }
                    else
                    {
                        hasToken = true;
                    }
                }
            }
            else if (parms.Length != (handlerHasService ? 3 : 2))
            {
                throw new InvalidDecoratorDelegateException();
            }
        }

        public void ResolveMethodHandlerProvider()
        {
            if (output is null || inputType is null)
                throw new InvalidOperationException("Read parameters is required.");

            resolution = new GenericResolution(inputType,
                output.OutputType, output.IsAsync, output.HasOutput,
                method);
        }

        public DecoratorDescription BuildDecoratorDescription()
        {
            if (kind is not ChainKind.Decorator)
                throw new InvalidOperationException("The builder is not for decorator.");

            if (output is null || inputType is null)
                throw new InvalidOperationException("Read decorator parameters is required.");

            Func<Type, Type, Delegate> provider;
            if (handler is not null)
                provider = (_, _) => handler;
            else if (resolution is not null)
                provider = resolution.CreateDelegate;
            else
                throw new InvalidOperationException("Handler provider can't be resolved");

            return new DecoratorDescription(inputType, output.OutputType, provider)
            {
                HasToken = hasToken,
                HasOutput = output.HasOutput,
                IsAsync = output.IsAsync
            };
        }

        #endregion

        #region Bridge Functions

        public void ReadBridgeParameters()
        {
            // first check parameters
            if (handlerHasService)
            {
                if (parms.Length is not 3 and not 4)
                    throw new InvalidDecoratorDelegateException();

                // check that first param must be the service
                if (parms[0].ParameterType.Implements(serviceType!))
                {
                    throw new InvalidServiceHandlerDelegateException();
                }
            }
            else
            {
                if (parms.Length is not 2 and not 3)
                    throw new InvalidDecoratorDelegateException();
            }

            // the input type.
            inputType = parms[handlerHasService ? 1 : 0].ParameterType;

            kind = ChainKind.Bridge;

            output = new OutputDescription(method);
        }

        public void ReadBridgeNextHandler()
        {
            if (output is null || inputType is null)
                throw new InvalidOperationException("Read bridge parameters is required.");

            var nextParm = parms[handlerHasService ? 2 : 1];
            var nextHandlerType = nextParm.ParameterType;

            if (!nextHandlerType.IsGenericType)
                throw new InvalidOperationException("TODO: criar exception do handler de bridge inválido");

            var genericHandlerType = nextHandlerType.GetGenericTypeDefinition();
            if (genericHandlerType != typeof(Action<>) && genericHandlerType != typeof(Func<,>))
                throw new InvalidOperationException("TODO: criar exception do handler de bridge inválido");

            var nextHandlerArguments = nextHandlerType.GetGenericArguments();
            var nextInput = nextHandlerArguments[0];

            if (genericHandlerType == typeof(Action<>))
            {
                bridgeNextHandlerDescription = new(nextInput, typeof(void), false, false);
            }
            else
            {
                // so, is Func<,>

                var nextOutput = nextHandlerArguments[1];
                if (nextOutput.Implements(typeof(Task)))
                {
                    if (nextOutput.IsGenericType)
                        bridgeNextHandlerDescription = new(nextInput, nextOutput.GetGenericArguments()[0], true, true);
                    else
                        bridgeNextHandlerDescription = new(nextInput, nextOutput, false, true);
                }
                else
                {
                    bridgeNextHandlerDescription = new(nextInput, nextOutput, true, false);
                }
            }
        }

        public void ValidateBridgeParameters()
        {
            if (output is null)
                throw new InvalidOperationException("Read bridge parameters is required.");

            if (bridgeNextHandlerDescription is null)
                throw new InvalidOperationException("Read bridge next handler is required.");

            // check parameters
            if (output.IsAsync)
            {
                if (parms.Length == (handlerHasService ? 4 : 3))
                {
                    if (parms[handlerHasService ? 3 : 2].ParameterType != typeof(CancellationToken))
                    {
                        throw new InvalidDecoratorDelegateException();
                    }
                    else
                    {
                        hasToken = true;
                    }
                }
            }
            else if (parms.Length != (handlerHasService ? 3 : 2))
            {
                throw new InvalidDecoratorDelegateException();
            }
        }

        public BridgeDescription BuildBridgeDescription()
        {

            if (kind is not ChainKind.Bridge)
                throw new InvalidOperationException("The builder is not for decorator.");

            if (output is null || inputType is null)
                throw new InvalidOperationException("Read bridge parameters is required.");

            if (bridgeNextHandlerDescription is null)
                throw new InvalidOperationException("Read bridge next handler is required.");

            Func<Type, Type, Delegate> provider;
            if (handler is not null)
                provider = (_, _) => handler;
            else if (resolution is not null)
                provider = resolution.CreateDelegate;
            else
                throw new InvalidOperationException("Handler provider can't be resolved");

            return new BridgeDescription(inputType, output.OutputType, provider, bridgeNextHandlerDescription)
            {
                HasToken = hasToken,
                HasOutput = output.HasOutput,
                IsAsync = output.IsAsync
            };

        }

        #endregion
    }
}
