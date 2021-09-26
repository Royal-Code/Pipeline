using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainDelegateAsync<TIn> : Chain<TIn>
    {
        private readonly Func<TIn, CancellationToken, Task> function;

        public HandlerChainDelegateAsync(IChainDelegateProvider<Func<TIn, CancellationToken, Task>> function)
        {
            this.function = function?.Delegate ?? throw new ArgumentNullException(nameof(function));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(input, default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token) => function(input, token);
    }

    public class HandlerChainDelegateAsync<TIn, TOut> : Chain<TIn, TOut>
    {
        private readonly Func<TIn, CancellationToken, Task<TOut>> function;

        public HandlerChainDelegateAsync(IChainDelegateProvider<Func<TIn, CancellationToken, Task<TOut>>> function)
        {
            this.function = function?.Delegate ?? throw new ArgumentNullException(nameof(function));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) 
            => function(input, default).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input, token);
    }
}
