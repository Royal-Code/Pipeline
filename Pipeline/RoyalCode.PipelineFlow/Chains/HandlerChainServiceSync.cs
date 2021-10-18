using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainServiceSync<TIn, TService> : Chain<TIn>
    {
        private readonly TService service;
        private readonly Action<TService, TIn> function;

        public HandlerChainServiceSync(IChainDelegateProvider<Action<TService, TIn>> functionProvider, TService service)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.service = service;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(service, input);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            function(service, input);
            return Task.CompletedTask;
        }
    }

    public class HandlerChainServiceSync<TIn, TOut, TService> : Chain<TIn, TOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, TOut> function;

        public HandlerChainServiceSync(
            IChainDelegateProvider<Func<TService, TIn, TOut>> functionProvider,
            TService service)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.service = service;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(service, input);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
        {
            var result = function(service, input);
            return Task.FromResult(result);
        }
    }
}
