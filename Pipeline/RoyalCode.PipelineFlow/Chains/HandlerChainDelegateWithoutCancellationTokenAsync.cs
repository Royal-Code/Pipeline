using RoyalCode.PipelineFlow.Configurations;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token) => function(input);
    }

    public class HandlerChainDelegteWithoutCancellationTokenAsync<TIn, TOut> : Chain<TIn, TOut>
    {
        private readonly Func<TIn, Task<TOut>> function;

        public HandlerChainDelegteWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TIn, Task<TOut>>> functionProvider)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(input).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input);
    }
}
