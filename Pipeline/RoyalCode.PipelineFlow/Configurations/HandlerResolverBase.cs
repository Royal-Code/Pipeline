using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public abstract class HandlerResolverBase : IHandlerResolver
    {
        private readonly HandlerDescription handlerDescription;

        protected HandlerResolverBase(HandlerDescription handlerDescription)
        {
            this.handlerDescription = handlerDescription ?? throw new ArgumentNullException(nameof(handlerDescription));
        }

        public bool IsFallback { get; protected set; }

        public HandlerDescription? TryResolve(Type inputType)
        {
            return handlerDescription.Match(inputType)
                ? handlerDescription
                : null;
        }

        public HandlerDescription? TryResolve(Type inputType, Type output)
        {
            return handlerDescription.Match(inputType, output)
                ? handlerDescription
                : null;
        }
    }
}
