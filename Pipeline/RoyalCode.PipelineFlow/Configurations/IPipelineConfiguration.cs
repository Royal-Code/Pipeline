using RoyalCode.PipelineFlow.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineConfiguration
    {
        HandlerRegistry Handlers { get; }
        
        DecoratorRegistry Decorators { get; }
    }

    public interface IPipelineConfiguration<TFor> : IPipelineConfiguration
    {

    }

    public interface IChainBuilder
    {
        ChainKind Kind { get; }

        Type Build(HandlerDescription handlerDescription, Type previousChainType);
        Type Build(HandlerDescription handlerDescription);
    }

    public interface IHandlerResolver
    {
        HandlerDescription TryResolve(Type inputType);
    }

    public class DelegateHandlerResolver : IHandlerResolver
    {
        private readonly HandlerDescription handlerDescription;

        public DelegateHandlerResolver(Delegate handler)
        {
            handlerDescription = handler.GetHandlerDescription();
        }

        HandlerDescription IHandlerResolver.TryResolve(Type inputType)
        {
            return handlerDescription.InputType == inputType && !handlerDescription.HasOutput 
                ? handlerDescription 
                : null;
        }
    }

    public class ServiceAndDelegateHandlerResolver : IHandlerResolver
    {
        public ServiceAndDelegateHandlerResolver(Delegate handler)
        {

        }

        public HandlerDescription TryResolve(Type inputType)
        {
            throw new NotImplementedException();
        }
    }

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
                HandlerDelegate = handler
            };

            return description;
        }
    }
}
