using RoyalCode.PipelineFlow.Resolvers;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// A base implementation for <see cref="IPipelineBuilder"/>.
    /// </summary>
    public abstract class PipelineBuilderBase : IPipelineBuilder
    {
        protected readonly IPipelineBuilder pipelineBuilder;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="pipelineBuilder">The previous builder.</param>
        /// <exception cref="ArgumentNullException">
        ///     Case <paramref name="pipelineBuilder"/> is null.
        /// </exception>
        internal protected PipelineBuilderBase(IPipelineBuilder pipelineBuilder)
        {
            this.pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));
        }

        /// <inheritdoc/>
        public void AddHandlerResolver(IHandlerResolver resolver) => pipelineBuilder.AddHandlerResolver(resolver);

        /// <inheritdoc/>
        public void AddDecoratorResolver(IDecoratorResolver resolver) => pipelineBuilder.AddDecoratorResolver(resolver);

        /// <inheritdoc/>
        public IPipelineBuilder<TInput> Configure<TInput>() => pipelineBuilder.Configure<TInput>();

        /// <inheritdoc/>
        public IPipelineBuilder<TInput, TOut> Configure<TInput, TOut>() => pipelineBuilder.Configure<TInput, TOut>();
    }
}
