
namespace RoyalCode.SmartPipelines.Configurations;

/// <summary>
/// Used to start building a <see cref="IPipelineFactory{TFor}"/>.
/// </summary>
public static class PipelineFactory
{
    /// <summary>
    /// The configuration for build a <see cref="IPipelineFactory{TFor}"/>.
    /// </summary>
    /// <typeparam name="TFor">The specific type of the pipeline.</typeparam>
    /// <returns>a new instance of <see cref="PipelineFactoryConfiguration{TFor}"/>.</returns>
    public static PipelineFactoryConfiguration<TFor> Configure<TFor>() => new(ChainDelegateRegistry);
}
