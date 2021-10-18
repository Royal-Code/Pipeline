using RoyalCode.PipelineFlow.Resolvers;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// Default implementation of <see cref="IPipelineBuilder"/>.
    /// </summary>
    public class DefaultPipelineBuilder : IPipelineBuilder
    {
        private readonly IPipelineConfiguration configuration;

        /// <summary>
        /// Create a new instance with the <see cref="IPipelineConfiguration"/> where the handlers will be stored.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="configuration"/> is null.
        /// </exception>
        public DefaultPipelineBuilder(IPipelineConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public void AddHandlerResolver(IHandlerResolver resolver)
        {
            configuration.Handlers.Add(resolver);
        }

        /// <inheritdoc/>
        public void AddDecoratorResolver(IDecoratorResolver resolver)
        {
            configuration.Decorators.Add(resolver);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TIn> Configure<TIn>() => new DefaultPipelineBuilder<TIn>(this);

        /// <inheritdoc/>
        public IPipelineBuilder<TIn, TOut> Configure<TIn, TOut>() => new DefaultPipelineBuilder<TIn, TOut>(this);
    }

    /// <summary>
    /// Default implementation of <see cref="IPipelineBuilder{TIn}"/>.
    /// </summary>
    public class DefaultPipelineBuilder<TIn> : PipelineBuilderBase, IPipelineBuilder<TIn>
    {
        /// <inheritdoc/>
        public DefaultPipelineBuilder(IPipelineBuilder pipelineBuilder)
            : base(pipelineBuilder)
        { }

        /// <inheritdoc/>
        public IPipelineBuilderWithService<TService, TIn> WithService<TService>() 
            => new DefaultPipelineBuilderWithService<TService, TIn>(pipelineBuilder);
    }

    /// <summary>
    /// Default implementation of <see cref="IPipelineBuilder{TIn, TOut}"/>.
    /// </summary>
    public class DefaultPipelineBuilder<TIn, TOut> : PipelineBuilderBase, IPipelineBuilder<TIn, TOut>
    {
        /// <inheritdoc/>
        public DefaultPipelineBuilder(IPipelineBuilder pipelineBuilder) 
            : base(pipelineBuilder) 
        { }

        /// <inheritdoc/>
        public IPipelineBuilderWithService<TService, TIn, TOut> WithService<TService>()
            => new DefaultPipelineBuilderWithService<TService, TIn, TOut>(pipelineBuilder);
    }
}
