using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    public class DelegateHandlerResolver : HandlerResolverBase
    {
        public DelegateHandlerResolver(Delegate handler)
            : base(handler.GetHandlerDescription())
        { }
    }
}
