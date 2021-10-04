using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    public class DelegateBridgeHandlerResolver : HandlerResolverBase
    {
        public DelegateBridgeHandlerResolver(Delegate handler)
            : base(handler.GetBridgeDescription())
        { }
    }
}
