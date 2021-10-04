using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class DelegateHandlerResolver : HandlerResolverBase
    {
        public DelegateHandlerResolver(Delegate handler)
            : base(handler.GetHandlerDescription())
        { }
    }
}
