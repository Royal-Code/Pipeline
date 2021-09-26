using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainServiceWithoutCancellationTokenAsync<TIn, TService> : Chain<TIn>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Task> function;

        public HandlerChainServiceWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TService, TIn, Task>> functionProvider,
            TService service)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider)); ;
            this.service = service;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(service, input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token) => function(service, input);
    }

    public class HandlerChainServiceWithoutCancellationTokenAsync<TIn, TOut, TService> : Chain<TIn, TOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Task<TOut>> function;

        public HandlerChainServiceWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TService, TIn, Task<TOut>>> functionProvider,
            TService service)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.service = service;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(service, input).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(service, input);
    }
}
