using RoyalCode.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class DecoratorChainServiceSync<TIn, TNext, TService> : DecoratorChain<TIn, TNext>
        where TNext : Chain<TIn>
    {
        private readonly TService service;
        private readonly Action<TService, TIn, Action> action;
        private readonly TNext next;

        public DecoratorChainServiceSync(TService service, Action<TService, TIn, Action> action, TNext next)
        {
            this.service = service;
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => action(service, input, () => next.Send(input));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            Task resultTask = null;
            action(service, input, () => { resultTask = next.SendAsync(input, token); });

            return resultTask ?? Task.CompletedTask;
        }
    }

    public class DecoratorChainServiceSync<TIn, TOut, TNext, TService> : DecoratorChain<TIn, TOut, TNext>
        where TNext : Chain<TIn, TOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<TOut>, TOut> function;
        private readonly TNext next;

        public DecoratorChainServiceSync(TService service, Func<TService, TIn, Func<TOut>, TOut> function, TNext next)
        {
            this.service = service;
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(service, input, () => next.Send(input));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => Task.FromResult(function(service, input, () => next.SendAsync(input, token).GetResultSynchronously()));
    }
}
