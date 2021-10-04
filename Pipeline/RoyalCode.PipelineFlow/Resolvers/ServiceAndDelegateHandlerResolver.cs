using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    public class ServiceAndDelegateHandlerResolver : HandlerResolverBase
    {
        public ServiceAndDelegateHandlerResolver(Delegate handler, Type serviceType)
            : base(handler.GetHandlerDescription(serviceType))
        { }
    }
}
