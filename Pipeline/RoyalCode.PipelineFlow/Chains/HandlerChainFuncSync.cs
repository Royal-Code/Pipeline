using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainFuncSync<TIn, TOut> : Chain<TIn, TOut>
    {
        private readonly Func<TIn, TOut> function;

        public HandlerChainFuncSync(Func<TIn, TOut> function)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
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
