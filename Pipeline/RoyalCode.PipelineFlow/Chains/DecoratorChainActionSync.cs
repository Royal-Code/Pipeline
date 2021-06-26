using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class DecoratorChainActionSync<TIn> : DecoratorChain<TIn>
    {
        private readonly Action<TIn, Action> action;

        public DecoratorChainActionSync(Action<TIn, Action> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input, Action next) => action(input, next);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, Func<Task> next, CancellationToken token)
        {
            Task resultTask = null;
            action(input, () => { resultTask = next(); });

            return resultTask ?? Task.CompletedTask;
        }
    }
}
