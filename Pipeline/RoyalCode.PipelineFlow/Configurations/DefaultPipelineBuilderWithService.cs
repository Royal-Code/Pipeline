namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// Default implementation of <see cref="IPipelineBuilderWithService{TService}"/>.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    public class DefaultPipelineBuilderWithService<TService> : PipelineBuilderBase, IPipelineBuilderWithService<TService>
    {
        /// <inheritdoc/>
        public DefaultPipelineBuilderWithService(IPipelineBuilder builder)
            : base(builder)
        { }
    }

    /// <summary>
    /// Default implementation of <see cref="IPipelineBuilderWithService{TService, TIn}"/>.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <typeparam name="TIn">The input type.</typeparam>
    public class DefaultPipelineBuilderWithService<TService, TIn> : DefaultPipelineBuilder<TIn>, IPipelineBuilderWithService<TService, TIn>
    {
        /// <inheritdoc/>
        public DefaultPipelineBuilderWithService(IPipelineBuilder pipelineBuilder) : base(pipelineBuilder) { }
    }

    /// <summary>
    /// Default implementation of <see cref="IPipelineBuilderWithService{TService, TIn, TOut}"/>.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    public class DefaultPipelineBuilderWithService<TService, TIn, TOut> : DefaultPipelineBuilder<TIn, TOut>, IPipelineBuilderWithService<TService, TIn, TOut>
    {
        /// <inheritdoc/>
        public DefaultPipelineBuilderWithService(IPipelineBuilder pipelineBuilder) : base(pipelineBuilder) { }
    }
}
