using System;

namespace RoyalCode.PipelineFlow.Chains
{
    public class PipelineHandlerChainServiceSync<TIn, TService> : HandlerChainServiceSync<TIn, TService>, IPipeline<TIn>
    {
        public PipelineHandlerChainServiceSync(Action<TService, TIn> action, TService service) : base(action, service) { }
    }

    public class PipelineHandlerChainServiceSync<TIn, TOut, TService> : HandlerChainServiceSync<TIn, TOut, TService>, IPipeline<TIn, TOut>
    {
        public PipelineHandlerChainServiceSync(Func<TService, TIn, TOut> function, TService service) 
            : base(function, service)
        { }
    }
}
