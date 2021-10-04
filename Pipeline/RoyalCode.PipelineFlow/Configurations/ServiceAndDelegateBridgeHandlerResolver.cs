using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class ServiceAndDelegateBridgeHandlerResolver : HandlerResolverBase
    {
        public ServiceAndDelegateBridgeHandlerResolver(Delegate handler, Type serviceType)
            : base(handler.GetBridgeDescription(serviceType))
        { }
    }
}
