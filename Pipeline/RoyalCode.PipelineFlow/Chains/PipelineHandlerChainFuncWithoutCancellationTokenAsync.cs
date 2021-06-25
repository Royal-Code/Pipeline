using System;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class PipelineHandlerChainFuncWithoutCancellationTokenAsync<TIn>
        : HandlerChainFuncWithoutCancellationTokenAsync<TIn>, IPipeline<TIn>
    {
        public PipelineHandlerChainFuncWithoutCancellationTokenAsync(Func<TIn, Task> function) : base(function) { }
    }

    public class PipelineHandlerChainFuncWithoutCancellationTokenAsync<TIn, TOut>
        : HandlerChainFuncWithoutCancellationTokenAsync<TIn, TOut>, IPipeline<TIn, TOut>
    {
        public PipelineHandlerChainFuncWithoutCancellationTokenAsync(Func<TIn, Task<TOut>> function) : base(function) { }
    }
}
