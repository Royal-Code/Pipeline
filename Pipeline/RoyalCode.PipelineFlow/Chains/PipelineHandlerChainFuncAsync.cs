using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class PipelineHandlerChainFuncAsync<TIn> : HandlerChainFuncAsync<TIn>, IPipeline<TIn>
    {
        public PipelineHandlerChainFuncAsync(Func<TIn, CancellationToken, Task> function) : base(function) { }
    }

    public class PipelineHandlerChainFuncAsync<TIn, TOut> : HandlerChainFuncAsync<TIn, TOut>, IPipeline<TIn, TOut>
    {
        public PipelineHandlerChainFuncAsync(Func<TIn, CancellationToken, Task<TOut>> function) 
            : base(function)
        { }
    }
}
