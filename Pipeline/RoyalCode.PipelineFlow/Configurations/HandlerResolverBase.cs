using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public abstract class HandlerResolverBase : IHandlerResolver
    {
        private readonly HandlerDescriptor handlerDescription;

        protected HandlerResolverBase(HandlerDescriptor handlerDescription)
        {
            this.handlerDescription = handlerDescription ?? throw new ArgumentNullException(nameof(handlerDescription));
        }

        public bool IsFallback { get; protected set; }

        public HandlerDescriptor? TryResolve(Type inputType)
        {
            return handlerDescription.Match(inputType)
                ? handlerDescription
                : null;
        }

        public HandlerDescriptor? TryResolve(Type inputType, Type output)
        {
            return handlerDescription.Match(inputType, output)
                ? handlerDescription
                : null;
        }
    }
}
