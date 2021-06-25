using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class HandlerChainServiceSync<TIn, TService> : Chain<TIn>
    {
        private readonly TService service;
        private readonly Action<TService, TIn> action;

        public HandlerChainServiceSync(Action<TService, TIn> action, TService service)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.service = service;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => action(service, input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            action(service, input);
            return Task.CompletedTask;
        }
    }

    public class HandlerChainServiceSync<TIn, TOut, TService> : Chain<TIn, TOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, TOut> function;

        public HandlerChainServiceSync(Func<TService, TIn, TOut> function, TService service)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.service = service;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(service, input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
        {
            var result = function(service, input);
            return Task.FromResult(result);
        }
    }
}
