using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class DecoratorChainDelegateWithoutCancellationTokenAsync<TIn, TNext> : DecoratorChain<TIn, TNext>
        where TNext : Chain<TIn>
    {
        private readonly Func<TIn, Func<Task>, Task> function;
        private readonly TNext next;

        public DecoratorChainDelegateWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TIn, Func<Task>, Task>> functionProvider,
            TNext next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input)
            => function(input, () => { next.Send(input); return Task.CompletedTask; });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
            => function(input, () => next.SendAsync(input, token));
    }

    public class DecoratorChainDelegateWithoutCancellationTokenAsync<TIn, TOut, TNext> : DecoratorChain<TIn, TOut, TNext>
        where TNext : Chain<TIn, TOut>
    {
        private readonly Func<TIn, Func<Task<TOut>>, Task<TOut>> function;
        private readonly TNext next;

        public DecoratorChainDelegateWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TIn, Func<Task<TOut>>, Task<TOut>>> functionProvider, 
            TNext next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input, () => Task.FromResult(next.Send(input))).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input, () => next.SendAsync(input, token));
    }
}
