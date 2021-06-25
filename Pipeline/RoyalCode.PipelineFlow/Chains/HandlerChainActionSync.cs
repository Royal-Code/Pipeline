using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainActionSync<TIn> : Chain<TIn>
    {
        private readonly Action<TIn> action;

        public HandlerChainActionSync(Action<TIn> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => action(input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            action(input);
            return Task.CompletedTask;
        }
    }
}
