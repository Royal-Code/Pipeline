using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class DecoratorChainActionSync<TIn, TNext> : DecoratorChain<TIn, TNext>
        where TNext : Chain<TIn>
    {
        private readonly Action<TIn, Action> action;
        private readonly TNext next;

        public DecoratorChainActionSync(Action<TIn, Action> action, TNext next)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => action(input, () => next.Send(input));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            Task? resultTask = null;
            action(input, () => { resultTask = next.SendAsync(input, token); });
            return resultTask ?? Task.CompletedTask;
        }
    }
}
