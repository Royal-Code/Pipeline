using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IHandlerResolver
    {
        HandlerDescription TryResolve(Type inputType);

        HandlerDescription TryResolve(Type inputType, Type output);
    }

    public abstract class HandlerResolverBase : IHandlerResolver
    {
        private readonly HandlerDescription handlerDescription;

        protected HandlerResolverBase(HandlerDescription handlerDescription)
        {
            this.handlerDescription = handlerDescription ?? throw new ArgumentNullException(nameof(handlerDescription));
        }

        public HandlerDescription TryResolve(Type inputType)
        {
            return handlerDescription.Match(inputType)
                ? handlerDescription
                : null;
        }

        public HandlerDescription TryResolve(Type inputType, Type output)
        {
            return handlerDescription.Match(inputType, output)
                ? handlerDescription
                : null;
        }
    }

    public class DelegateHandlerResolver : HandlerResolverBase
    {
        public DelegateHandlerResolver(Delegate handler)
            : base(handler.GetHandlerDescription())
        { }
    }

    public class ServiceAndDelegateHandlerResolver : HandlerResolverBase
    {
        public ServiceAndDelegateHandlerResolver(Delegate handler, Type serviceType)
            : base(handler.GetHandlerDescription(serviceType))
        { }
    }

    public class MethodHandlerResolver : HandlerResolverBase
    {
        public MethodHandlerResolver(MethodInfo methodHandler)
            : base(methodHandler.GetHandlerDescription())
        { }
    }
}
