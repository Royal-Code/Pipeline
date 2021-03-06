﻿using RoyalCode.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class DecoratorChainFuncAsync<TIn, TNext> : DecoratorChain<TIn, TNext>
        where TNext : Chain<TIn>
    {
        private readonly Func<TIn, Func<Task>, CancellationToken, Task> function;
        private readonly TNext next;

        public DecoratorChainFuncAsync(Func<TIn, Func<Task>, CancellationToken, Task> function, TNext next)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) 
            => function(input, () => { next.Send(input); return Task.CompletedTask; }, default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token) 
            => function(input, () => next.SendAsync(input, token), token);
    }

    public class DecoratorChainFuncAsync<TIn, TOut, TNext> : DecoratorChain<TIn, TOut, TNext>
        where TNext : Chain<TIn, TOut>
    {
        private readonly Func<TIn, Func<Task<TOut>>, CancellationToken, Task<TOut>> function;
        private readonly TNext next;

        public DecoratorChainFuncAsync(Func<TIn, Func<Task<TOut>>, CancellationToken, Task<TOut>> function, TNext next)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input) => function(input, () => Task.FromResult(next.Send(input)), default).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input, () => next.SendAsync(input, token), token);
    }
}
