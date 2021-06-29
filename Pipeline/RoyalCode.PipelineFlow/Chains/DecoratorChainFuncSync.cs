using RoyalCode.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class DecoratorChainFuncSync<TIn, TOut, TNext> : DecoratorChain<TIn, TOut, TNext>
        where TNext : Chain<TIn, TOut>
    {
        private readonly Func<TIn, Func<TOut>, TOut> function;
        private readonly TNext next;

        public DecoratorChainFuncSync(Func<TIn, Func<TOut>, TOut> function, TNext next)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input, () => next.Send(input));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => Task.FromResult(function(input, () => next.SendAsync(input, token).GetResultSynchronously()));
    }
}
