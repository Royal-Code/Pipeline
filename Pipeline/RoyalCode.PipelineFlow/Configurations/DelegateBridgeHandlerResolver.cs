using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class DelegateBridgeHandlerResolver : HandlerResolverBase
    {
        public DelegateBridgeHandlerResolver(Delegate handler)
            : base(handler.GetBridgeDescription())
        { }
    }
}
