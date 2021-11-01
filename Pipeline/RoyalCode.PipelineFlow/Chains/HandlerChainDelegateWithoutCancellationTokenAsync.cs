using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainDelegateWithoutCancellationTokenAsync<TIn> : Chain<TIn>
    {
        private readonly Func<TIn, Task> function;

        public HandlerChainDelegateWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TIn, Task>> functionProvider)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(input);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token) => function(input);
    }

    public class HandlerChainDelegateWithoutCancellationTokenAsync<TIn, TOut> : Chain<TIn, TOut>
    {
        private readonly Func<TIn, Task<TOut>> function;

        public HandlerChainDelegateWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TIn, Task<TOut>>> functionProvider)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(input).GetResultSynchronously();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input);
    }
}
