using System;

namespace RoyalCode.PipelineFlow.Chains
{
    public class PipelineHandlerChainFuncSync<TIn, TOut> : HandlerChainFuncSync<TIn, TOut>, IPipeline<TIn, TOut>
    {
        public PipelineHandlerChainFuncSync(Func<TIn, TOut> function) : base(function) { }
    }
}
