using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class BridgeChainServiceSync<TIn, TNextIn, TNextChain, TService> : BridgeChain<TIn, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn>
    {
        private readonly TService service;
        private readonly Action<TService, TIn, Action<TNextIn>> action;
        private readonly TNextChain next;

        public BridgeChainServiceSync(
            TService service,
            Action<TService, TIn, Action<TNextIn>> action,
            TNextChain next)
        {
            this.service = service;
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) => action(service, input, nextInput => next.Send(nextInput));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
        {
            action(service, input, nextInput => next.Send(nextInput));
            return Task.CompletedTask;
        }
    }

    public class BridgeChainServiceSync<TIn, TOut, TNextIn, TNextChain, TService>
        : BridgeChain<TIn, TOut, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn, TOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<TNextIn, TOut>, TOut> function;
        private readonly TNextChain next;

        public BridgeChainServiceSync(
            TService service,
            Func<TService, TIn, Func<TNextIn, TOut>, TOut> function,
            TNextChain next)
        {
            this.service = service;
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(service, input, nextInput => next.Send(nextInput));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => Task.FromResult(function(service, input, nextInput => next.Send(nextInput)));
    }

    public class BridgeChainServiceSync<TIn, TOut, TNextIn, TNextOut, TNextChain, TService>
        : BridgeChain<TIn, TOut, TNextIn, TNextOut, TNextChain>
        where TNextChain : Chain<TNextIn, TNextOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<TNextIn, TNextOut>, TOut> function;
        private readonly TNextChain next;

        public BridgeChainServiceSync(
            TService service,
            Func<TService, TIn, Func<TNextIn, TNextOut>, TOut> function,
            TNextChain next)
        {
            this.service = service;
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(service, input, nextInput => next.Send(nextInput));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => Task.FromResult(function(service, input, nextInput => next.Send(nextInput)));
    }
}
