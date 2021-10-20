using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// Bridge chain.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TNextIn">The next input type.</typeparam>
    /// <typeparam name="TNextChain">The next chain type.</typeparam>
    public class BridgeChainDelegateWithoutCancellationTokenAsync<TIn, TNextIn, TNextChain> : BridgeChain<TIn, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn>
    {
        private readonly Func<TIn, Func<TNextIn, Task>, Task> function;
        private readonly TNextChain next;

        /// <summary>
        /// Create the chain with the dependencies.
        /// </summary>
        /// <param name="functionProvider">Provides the delegate handler.</param>
        /// <param name="next">The next chain.</param>
        /// <exception cref="ArgumentNullException">
        ///     If any parameter is null.
        /// </exception>
        public BridgeChainDelegateWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TIn, Func<TNextIn, Task>, Task>> functionProvider,
            TNextChain next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Send(TIn input) 
            => function(input, nextInput => { next.Send(nextInput); return Task.CompletedTask; });

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task SendAsync(TIn input, CancellationToken token)
            => function(input, nextInput => next.SendAsync(nextInput, token));
    }

    /// <summary>
    /// Bridge chain.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <typeparam name="TNextIn">The next input type.</typeparam>
    /// <typeparam name="TNextChain">The next chain type.</typeparam>
    public class BridgeChainDelegateWithoutCancellationTokenAsync<TIn, TOut, TNextIn, TNextChain> : BridgeChain<TIn, TOut, TNextIn, TNextChain>
        where TNextChain : Chain<TNextIn, TOut>
    {
        private readonly Func<TIn, Func<TNextIn, Task<TOut>>, Task<TOut>> function;
        private readonly TNextChain next;

        /// <summary>
        /// Create the chain with the dependencies.
        /// </summary>
        /// <param name="functionProvider">Provides the delegate handler.</param>
        /// <param name="next">The next chain.</param>
        /// <exception cref="ArgumentNullException">
        ///     If any parameter is null.
        /// </exception>
        public BridgeChainDelegateWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TIn, Func<TNextIn, Task<TOut>>, Task<TOut>>> functionProvider,
            TNextChain next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(input, nextInput => Task.FromResult(next.Send(nextInput))).GetResultSynchronously();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input, nextInput => next.SendAsync(nextInput, token));
    }

    /// <summary>
    /// Bridge chain.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <typeparam name="TNextIn">The next input type.</typeparam>
    /// <typeparam name="TNextOut">The output input type.</typeparam>
    /// <typeparam name="TNextChain">The next chain type.</typeparam>
    public class BridgeChainDelegateWithoutCancellationTokenAsync<TIn, TOut, TNextIn, TNextOut, TNextChain>
        : BridgeChain<TIn, TOut, TNextIn, TNextOut, TNextChain>
        where TNextChain : Chain<TNextIn, TNextOut>
    {
        private readonly Func<TIn, Func<TNextIn, Task<TNextOut>>, Task<TOut>> function;
        private readonly TNextChain next;

        /// <summary>
        /// Create the chain with the dependencies.
        /// </summary>
        /// <param name="functionProvider">Provides the delegate handler.</param>
        /// <param name="next">The next chain.</param>
        /// <exception cref="ArgumentNullException">
        ///     If any parameter is null.
        /// </exception>
        public BridgeChainDelegateWithoutCancellationTokenAsync(
            IChainDelegateProvider<Func<TIn, Func<TNextIn, Task<TNextOut>>, Task<TOut>>> functionProvider,
            TNextChain next)
        {
            function = functionProvider?.Delegate ?? throw new ArgumentNullException(nameof(functionProvider));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TOut Send(TIn input)
            => function(input, nextInput => Task.FromResult(next.Send(nextInput))).GetResultSynchronously();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Task<TOut> SendAsync(TIn input, CancellationToken token)
            => function(input, nextInput => next.SendAsync(nextInput, token));
    }
}
