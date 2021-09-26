using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class BridgeChainDelegateSync<TIn, TNextIn, TNextChain> : BridgeChain<TIn, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn>
    {
        private readonly Action<TIn, Action<TNextIn>> action;
        private readonly TNextChain next;

        public BridgeChainDelegateSync(Action<TIn, Action<TNextIn>> action, TNextChain next)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => action(input, nextInput => next.Send(nextInput));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            action(input, nextInput => next.Send(nextInput));
            return Task.CompletedTask;
        }
    }

    public class BridgeChainDelegateSync<TIn, TOut, TNextIn, TNextChain>
        : BridgeChain<TIn, TOut, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn, TOut>
    {
        private readonly Func<TIn, Func<TNextIn, TOut>, TOut> function;
        private readonly TNextChain next;

        public BridgeChainDelegateSync(Func<TIn, Func<TNextIn, TOut>, TOut> function, TNextChain next)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input, nextInput => next.Send(nextInput));

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

        public BridgeChainDelegateSync(Func<TIn, Func<TNextIn, TNextOut>, TOut> function, TNextChain next)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input, nextInput => next.Send(nextInput));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => Task.FromResult(function(input, nextInput => next.Send(nextInput)));
    }
}
