using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainFuncWithoutCancellationTokenAsync<TIn> : Chain<TIn>
    {
        private readonly Func<TIn, Task> function;

        public HandlerChainFuncWithoutCancellationTokenAsync(Func<TIn, Task> function)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token) => function(input);
    }

    public class HandlerChainFuncWithoutCancellationTokenAsync<TIn, TOut> : Chain<TIn, TOut>
    {
        private readonly Func<TIn, Task<TOut>> function;

        public HandlerChainFuncWithoutCancellationTokenAsync(Func<TIn, Task<TOut>> function)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(input).ConfigureAwait(false).GetAwaiter().GetResult();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input);
    }
}
