using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainDelegateSync<TIn> : Chain<TIn>
    {
        private readonly Action<TIn> function;

        public HandlerChainDelegateSync(IChainDelegateProvider<Action<TIn>> functionProvider)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            function(input);
            return Task.CompletedTask;
        }
    }

    public class HandlerChainDelegateSync<TIn, TOut> : Chain<TIn, TOut>
    {
        private readonly Func<TIn, TOut> function;

        public HandlerChainDelegateSync(IChainDelegateProvider<Func<TIn, TOut>> functionProvider)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
        {
            var result = function(input);
            return Task.FromResult(result);
        }
    }
}
