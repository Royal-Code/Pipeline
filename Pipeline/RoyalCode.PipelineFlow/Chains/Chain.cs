using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// Abstraction of a Pipeline Chain.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    public abstract class Chain<TIn> : IPipeline<TIn>
    {
        /// <summary>
        /// Sends a request to be processed by a handler.
        /// </summary>
        /// <param name="input">Input object for processing.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public abstract void Send(TIn input);

        /// <summary>
        /// Sends a request to be processed by a handler.
        /// </summary>
        /// <param name="input">Input object for processing.</param>
        /// <param name="token">Token for cancellation.</param>
        /// <returns>Task for asynchronous processing.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public abstract Task SendAsync(TIn input, CancellationToken token);
    }

    /// <summary>
    /// Abstraction of a Pipeline Chain.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output/result type.</typeparam>
    public abstract class Chain<TIn, TOut> : IPipeline<TIn, TOut>
    {
        /// <summary>
        /// Sends a request to be processed by a handler.
        /// </summary>
        /// <param name="input">Input object for processing.</param>
        /// <returns>Result of requisition processing.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public abstract TOut Send(TIn input);

        /// <summary>
        /// Sends a request to be processed by a handler.
        /// </summary>
        /// <param name="input">Input object for processing.</param>
        /// <param name="token">Token for cancellation.</param>
        /// <returns>Result of requisition processing.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public abstract Task<TOut> SendAsync(TIn input, CancellationToken token);
    }
}
