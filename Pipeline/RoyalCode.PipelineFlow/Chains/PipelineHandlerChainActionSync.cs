using System;

namespace RoyalCode.PipelineFlow.Chains
{
    public class PipelineHandlerChainActionSync<TIn> : HandlerChainActionSync<TIn>, IPipeline<TIn>
    {
        public PipelineHandlerChainActionSync(Action<TIn> action) : base(action) { }
    }
}
