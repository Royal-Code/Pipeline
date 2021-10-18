using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class BridgeChainDelegateSync<TIn, TNextIn, TNextChain> : BridgeChain<TIn, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn>
    {
        private readonly Action<TIn, Action<TNextIn>> function;
        private readonly TNextChain next;

        public BridgeChainDelegateSync(
            IChainDelegateProvider<Action<TIn, Action<TNextIn>>> functionProvider,
            TNextChain next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(input, nextInput => next.Send(nextInput));

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            function(input, nextInput => next.Send(nextInput));
            return Task.CompletedTask;
        }
    }

    public class BridgeChainDelegateSync<TIn, TOut, TNextIn, TNextChain>
        : BridgeChain<TIn, TOut, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn, TOut>
    {
        private readonly Func<TIn, Func<TNextIn, TOut>, TOut> function;
        private readonly TNextChain next;

        public BridgeChainDelegateSync(
            IChainDelegateProvider<Func<TIn, Func<TNextIn, TOut>, TOut>> functionProvider, 
            TNextChain next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input, nextInput => next.Send(nextInput));

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => Task.FromResult(function(input, nextInput => next.Send(nextInput)));
    }

    public class BridgeChainDelegateSync<TIn, TOut, TNextIn, TNextOut, TNextChain>
        : BridgeChain<TIn, TOut, TNextIn, TNextOut, TNextChain>
        where TNextChain : Chain<TNextIn, TNextOut>
    {
        private readonly Func<TIn, Func<TNextIn, TNextOut>, TOut> function;
        private readonly TNextChain next;

        public BridgeChainDelegateSync(
            IChainDelegateProvider<Func<TIn, Func<TNextIn, TNextOut>, TOut>> functionProvider,
            TNextChain next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input, nextInput => next.Send(nextInput));

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => Task.FromResult(function(input, nextInput => next.Send(nextInput)));
    }
}
