namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// Internal and default implementation for <see cref="IPipelineConfiguration{TFor}"/>.
    /// </summary>
    /// <typeparam name="TFor">The specific pipeline type.</typeparam>
    internal class PipelineConfiguration<TFor> : IPipelineConfiguration<TFor>
    {
        /// <inheritdoc/>
        public HandlerRegistry Handlers { get; } = new HandlerRegistry();

        /// <inheritdoc/>
        public DecoratorRegistry Decorators { get; } = new DecoratorRegistry();
    }
}
