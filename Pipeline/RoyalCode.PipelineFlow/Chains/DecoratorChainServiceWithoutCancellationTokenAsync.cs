using RoyalCode.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class DecoratorChainServiceWithoutCancellationTokenAsync<TIn, TNext, TService> : DecoratorChain<TIn, TNext>
        where TNext : Chain<TIn>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<Task>, Task> function;
        private readonly TNext next;

        public DecoratorChainServiceWithoutCancellationTokenAsync(
            TService service, Func<TService, TIn, Func<Task>, Task> function, TNext next)
        {
            this.service = service;
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input)
            => function(service, input, () => { next.Send(input); return Task.CompletedTask; });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
            => function(service, input, () => next.SendAsync(input, token));
    }

    public class DecoratorChainServiceWithoutCancellationTokenAsync<TIn, TOut, TNext, TService> : DecoratorChain<TIn, TOut, TNext>
        where TNext : Chain<TIn, TOut>
    {
        private readonly TService service;
        private readonly Func<TService, TIn, Func<Task<TOut>>, Task<TOut>> function;
        private readonly TNext next;

        public DecoratorChainServiceWithoutCancellationTokenAsync(
            TService service, Func<TService, TIn, Func<Task<TOut>>, Task<TOut>> function, TNext next)
        {
            this.service = service;
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) 
            => function(service, input, () => Task.FromResult(next.Send(input))).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(service, input, () => next.SendAsync(input, token));
    }
}
