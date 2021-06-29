using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class BridgeChainActionSync<TIn, TNextIn, TNextChain> : BridgeChain<TIn, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn>
    {
        private readonly Action<TIn, Action<TNextIn>> action;
        private readonly TNextChain next;

        public BridgeChainActionSync(Action<TIn, Action<TNextIn>> action, TNextChain next)
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
}
