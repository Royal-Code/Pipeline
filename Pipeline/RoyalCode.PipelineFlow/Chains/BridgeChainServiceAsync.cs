using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class BridgeChainServiceAsync<TIn, TNextIn, TNextChain, TService> : BridgeChain<TIn, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<TNextIn, Task>, CancellationToken, Task> function;
        private readonly TNextChain next;

        public BridgeChainServiceAsync(
            TService service,
            IChainDelegateProvider<Func<TService, TIn, Func<TNextIn, Task>, CancellationToken, Task>> functionProvider,
            TNextChain next)
        {
            this.service = service;
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input)
            => function(service, input, nextInput => { next.Send(nextInput); return Task.CompletedTask; }, default);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
            => function(service, input, nextInput => next.SendAsync(nextInput, token), token);
    }

    public class BridgeChainServiceAsync<TIn, TOut, TNextIn, TNextChain, TService> : BridgeChain<TIn, TOut, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn, TOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<TNextIn, Task<TOut>>, CancellationToken, Task<TOut>> function;
        private readonly TNextChain next;

        public BridgeChainServiceAsync(
            TService service,
            IChainDelegateProvider<Func<TService, TIn, Func<TNextIn, Task<TOut>>, CancellationToken, Task<TOut>>> functionProvider,
            TNextChain next)
        {
            this.service = service;
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(service, input, nextInput => Task.FromResult(next.Send(nextInput)), default).GetResultSynchronously();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(service, input, nextInput => next.SendAsync(nextInput, token), token);
    }

    public class BridgeChainServiceAsync<TIn, TOut, TNextIn, TNextOut, TNextChain, TService>
        : BridgeChain<TIn, TOut, TNextIn, TNextOut, TNextChain>
        where TNextChain : Chain<TNextIn, TNextOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<TNextIn, Task<TNextOut>>, CancellationToken, Task<TOut>> function;
        private readonly TNextChain next;

        public BridgeChainServiceAsync(
            TService service,
            IChainDelegateProvider<Func<TService, TIn, Func<TNextIn, Task<TNextOut>>, CancellationToken, Task<TOut>>> functionProvider,
            TNextChain next)
        {
            this.service = service;
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(service, input, nextInput => Task.FromResult(next.Send(nextInput)), default).GetResultSynchronously();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(service, input, nextInput => next.SendAsync(nextInput, token), token);
    }
}
