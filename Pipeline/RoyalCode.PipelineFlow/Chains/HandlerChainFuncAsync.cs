using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainFuncAsync<TIn> : Chain<TIn>
    {
        private readonly Func<TIn, CancellationToken, Task> function;

        public HandlerChainFuncAsync(Func<TIn, CancellationToken, Task> function)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => function(input, default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token) => function(input, token);
    }

    public class HandlerChainFuncAsync<TIn, TOut> : Chain<TIn, TOut>
    {
        private readonly Func<TIn, CancellationToken, Task<TOut>> function;

        public HandlerChainFuncAsync(Func<TIn, CancellationToken, Task<TOut>> function)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) 
            => function(input, default).ConfigureAwait(false).GetAwaiter().GetResult();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input, token);
    }
}
