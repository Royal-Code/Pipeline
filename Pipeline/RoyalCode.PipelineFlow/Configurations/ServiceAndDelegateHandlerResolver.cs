using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class ServiceAndDelegateHandlerResolver : HandlerResolverBase
    {
        public ServiceAndDelegateHandlerResolver(Delegate handler, Type serviceType)
            : base(handler.GetHandlerDescription(serviceType))
        { }
    }
}
