namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// <para>
    ///     The handler settings for creating pipelines.
    /// </para>
    /// <para>
    ///     These configurations are built with auxiliary classes, such as <see cref="IPipelineBuilder"/>.
    /// </para>
    /// </summary>
    public interface IPipelineConfiguration
    {
        /// <summary>
        /// The requests (input/output) processing handlers.
        /// </summary>
        HandlerRegistry Handlers { get; }

        /// <summary>
        /// The requests (input/output) decorators handlers.
        /// </summary>
        DecoratorRegistry Decorators { get; }
    }

    /// <summary>
    /// <para>
    ///     The configuration for some pipeline type.
    /// </para>
    /// <para>
    ///     These pipeline types can be, for example, ICommandQueryBus, IMessageBus, IHttpBus, 
    ///     or another component that can send requests (commands) to be processed.
    /// </para>
    /// </summary>
    /// <typeparam name="TFor">The specific pipeline type.</typeparam>
    public interface IPipelineConfiguration<TFor> : IPipelineConfiguration { }
}
