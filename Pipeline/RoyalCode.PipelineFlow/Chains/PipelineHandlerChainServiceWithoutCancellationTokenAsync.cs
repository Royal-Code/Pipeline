using System;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class PipelineHandlerChainServiceWithoutCancellationTokenAsync<TIn, TService> 
        : HandlerChainServiceWithoutCancellationTokenAsync<TIn, TService>, IPipeline<TIn>
    {
        public PipelineHandlerChainServiceWithoutCancellationTokenAsync(
            Func<TService, TIn, Task> function, TService service) 
            : base(function, service) { }
    }

    public class PipelineHandlerChainServiceWithoutCancellationTokenAsync<TIn, TOut, TService>
        : HandlerChainServiceWithoutCancellationTokenAsync<TIn, TOut, TService>, IPipeline<TIn, TOut>
    {
        public PipelineHandlerChainServiceWithoutCancellationTokenAsync(
            Func<TService, TIn, Task<TOut>> function, TService service)
            : base(function, service) { }
    }
}
