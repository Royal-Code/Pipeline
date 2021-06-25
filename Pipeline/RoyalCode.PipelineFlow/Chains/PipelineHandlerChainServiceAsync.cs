using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class PipelineHandlerChainServiceAsync<TIn, TService> : HandlerChainServiceAsync<TIn, TService>, IPipeline<TIn>
    {
        public PipelineHandlerChainServiceAsync(Func<TService, TIn, CancellationToken, Task> function, TService service) 
            : base(function, service) { }
    }

    public class PipelineHandlerChainServiceAsync<TIn, TOut, TService> 
        : HandlerChainServiceAsync<TIn, TOut, TService>, IPipeline<TIn, TOut>
    {
        public PipelineHandlerChainServiceAsync(
            Func<TService, TIn, CancellationToken, Task<TOut>> function, TService service) 
            : base(function, service)
        { }
    }
}
