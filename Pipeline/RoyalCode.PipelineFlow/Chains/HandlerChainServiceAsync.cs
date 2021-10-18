using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainServiceAsync<TIn, TService> : Chain<TIn>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, CancellationToken, Task> function;

        public HandlerChainServiceAsync(
            IChainDelegateProvider<Func<TService, TIn, CancellationToken, Task>> functionProvider,
            TService service)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.service = service;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(service, input, default);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token) => function(service, input, token);
    }

    public class HandlerChainServiceAsync<TIn, TOut, TService> : Chain<TIn, TOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, CancellationToken, Task<TOut>> functionProvider;

        public HandlerChainServiceAsync(
            IChainDelegateProvider<Func<TService, TIn, CancellationToken, Task<TOut>>> function,
            TService service)
        {
            functionProvider = function?.Delegate ?? throw new ArgumentNullException(nameof(function));
            this.service = service;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => functionProvider(service, input, default).GetResultSynchronously();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => functionProvider(service, input, token);
    }
}
