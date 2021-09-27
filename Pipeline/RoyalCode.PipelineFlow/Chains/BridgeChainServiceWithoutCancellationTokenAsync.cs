using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class BridgeChainServiceWithoutCancellationTokenAsync<TIn, TNextIn, TNextChain, TService>
        : BridgeChain<TIn, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<TNextIn, Task>, Task> function;
        private readonly TNextChain next;

        public BridgeChainServiceWithoutCancellationTokenAsync(
            TService service,
            IChainDelegateProvider<Func<TService, TIn, Func<TNextIn, Task>, Task>> functionProvider,
            TNextChain next)
        {
            this.service = service;
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input)
            => function(service, input, nextInput => { next.Send(nextInput); return Task.CompletedTask; });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
            => function(service, input, nextInput => next.SendAsync(nextInput, token));
    }

    public class BridgeChainServiceWithoutCancellationTokenAsync<TIn, TOut, TNextIn, TNextChain, TService>
        : BridgeChain<TIn, TOut, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn, TOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<TNextIn, Task<TOut>>, Task<TOut>> function;
        private readonly TNextChain next;

        public BridgeChainServiceWithoutCancellationTokenAsync(
            TService service,
            IChainDelegateProvider<Func<TService, TIn, Func<TNextIn, Task<TOut>>, Task<TOut>>> functionProvider,
            TNextChain next)
        {
            this.service = service;
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(service, input, nextInput => Task.FromResult(next.Send(nextInput))).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(service, input, nextInput => next.SendAsync(nextInput, token));
    }

    public class BridgeChainServiceWithoutCancellationTokenAsync<TIn, TOut, TNextIn, TNextOut, TNextChain, TService>
        : BridgeChain<TIn, TOut, TNextIn, TNextOut, TNextChain>
        where TNextChain : Chain<TNextIn, TNextOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<TNextIn, Task<TNextOut>>, Task<TOut>> function;
        private readonly TNextChain next;

        public BridgeChainServiceWithoutCancellationTokenAsync(
            TService service,
            IChainDelegateProvider<Func<TService, TIn, Func<TNextIn, Task<TNextOut>>, Task<TOut>>> functionProvider,
            TNextChain next)
        {
            this.service = service;
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(service, input, nextInput => Task.FromResult(next.Send(nextInput))).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(service, input, nextInput => next.SendAsync(nextInput, token));
    }
}
