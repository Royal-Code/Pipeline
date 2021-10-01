using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class DecoratorChainDelegateSync<TIn, TNext> : DecoratorChain<TIn, TNext>
        where TNext : Chain<TIn>
    {
        private readonly Action<TIn, Action> function;
        private readonly TNext next;

        public DecoratorChainDelegateSync(
            IChainDelegateProvider<Action<TIn, Action>> functionProvider, 
            TNext next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(input, () => next.Send(input));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            Task? resultTask = null;
            function(input, () => { resultTask = next.SendAsync(input, token); });
            return resultTask ?? Task.CompletedTask;
        }
    }

    public class DecoratorChainDelegateSync<TIn, TOut, TNext> : DecoratorChain<TIn, TOut, TNext>
        where TNext : Chain<TIn, TOut>
    {
        private readonly Func<TIn, Func<TOut>, TOut> function;
        private readonly TNext next;

        public DecoratorChainDelegateSync(
            IChainDelegateProvider<Func<TIn, Func<TOut>, TOut>> functionProvider,
            TNext next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input, () => next.Send(input));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => Task.FromResult(function(input, () => next.SendAsync(input, token).GetResultSynchronously()));
    }
}
