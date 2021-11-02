using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow
{
    /// <inheritdoc/>
    public class PipelineCaller<TChain, TIn> : IPipelineCaller
        where TChain : IPipeline<TIn>
    {
        private readonly TChain pipeline;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="pipeline">The real pipeline.</param>
        public PipelineCaller(TChain pipeline)
        {
            this.pipeline = pipeline;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send(object input) => pipeline.Send((TIn)input);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task SendAsync(object input, CancellationToken cancellationToken = default)
            => pipeline.SendAsync((TIn)input, cancellationToken);
    }

    /// <inheritdoc/>
    public class PipelineCaller<TChain, TIn, TOut> : IPipelineCaller<TOut>
        where TChain : IPipeline<TIn, TOut>
    {
        private readonly TChain pipeline;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="pipeline">The real pipeline.</param>
        public PipelineCaller(TChain pipeline)
        {
            this.pipeline = pipeline;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public TOut Send(object input) => pipeline.Send((TIn)input);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<TOut> SendAsync(object input, CancellationToken cancellationToken = default)
            => pipeline.SendAsync((TIn) input, cancellationToken);
    }
}
