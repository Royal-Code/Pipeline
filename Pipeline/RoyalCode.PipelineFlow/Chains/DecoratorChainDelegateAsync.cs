using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class DecoratorChainDelegateAsync<TIn, TNext> : DecoratorChain<TIn, TNext>
        where TNext : Chain<TIn>
    {
        private readonly Func<TIn, Func<Task>, CancellationToken, Task> function;
        private readonly TNext next;

        public DecoratorChainDelegateAsync(
            IChainDelegateProvider<Func<TIn, Func<Task>, CancellationToken, Task>> functionProvider,
            TNext next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) 
            => function(input, () => { next.Send(input); return Task.CompletedTask; }, default);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token) 
            => function(input, () => next.SendAsync(input, token), token);
    }

    public class DecoratorChainDelegateAsync<TIn, TOut, TNext> : DecoratorChain<TIn, TOut, TNext>
        where TNext : Chain<TIn, TOut>
    {
        private readonly Func<TIn, Func<Task<TOut>>, CancellationToken, Task<TOut>> function;
        private readonly TNext next;

        public DecoratorChainDelegateAsync(
            IChainDelegateProvider<Func<TIn, Func<Task<TOut>>, CancellationToken, Task<TOut>>> functionProvider, 
            TNext next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input, () => Task.FromResult(next.Send(input)), default).GetResultSynchronously();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input, () => next.SendAsync(input, token), token);
    }
}
