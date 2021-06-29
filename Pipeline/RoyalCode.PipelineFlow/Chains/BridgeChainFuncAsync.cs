﻿using RoyalCode.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    public class BridgeChainFuncAsync<TIn, TNextIn, TNextChain> : BridgeChain<TIn, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn>
    {
        private readonly Func<TIn, Func<TNextIn, Task>, CancellationToken, Task> function;
        private readonly TNextChain next;

        public BridgeChainFuncAsync(
            Func<TIn, Func<TNextIn, Task>, CancellationToken, Task> function,
            TNextChain next)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) 
            => function(input, nextInput => { next.Send(nextInput); return Task.CompletedTask; }, default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
            => function(input, nextInput => next.SendAsync(nextInput, token), token);
    }

    public class BridgeChainFuncAsync<TIn, TOut, TNextIn, TNextChain> : BridgeChain<TIn, TOut, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn, TOut>
    {
        private readonly Func<TIn, Func<TNextIn, Task<TOut>>, CancellationToken, Task<TOut>> function;
        private readonly TNextChain next;

        public BridgeChainFuncAsync(
            Func<TIn, Func<TNextIn, Task<TOut>>, CancellationToken, Task<TOut>> function,
            TNextChain next)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(input, nextInput => Task.FromResult(next.Send(nextInput)), default).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input, nextInput => next.SendAsync(nextInput, token), token);
    }

    public class BridgeChainFuncAsync<TIn, TOut, TNextIn, TNextOut, TNextChain>
        : BridgeChain<TIn, TOut, TNextIn, TNextOut, TNextChain>
        where TNextChain : Chain<TNextIn, TNextOut>
    {
        private readonly Func<TIn, Func<TNextIn, Task<TNextOut>>, CancellationToken, Task<TOut>> function;
        private readonly TNextChain next;

        public BridgeChainFuncAsync(
            Func<TIn, Func<TNextIn, Task<TNextOut>>, CancellationToken, Task<TOut>> function,
            TNextChain next)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(input, nextInput => Task.FromResult(next.Send(nextInput)), default).GetResultSynchronously();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input, nextInput => next.SendAsync(nextInput, token), token);
    }
}
