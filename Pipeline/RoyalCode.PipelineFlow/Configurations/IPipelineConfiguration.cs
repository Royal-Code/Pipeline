namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineConfiguration
    {
        HandlerRegistry Handlers { get; }

        DecoratorRegistry Decorators { get; }
    }

    public interface IPipelineConfiguration<TFor> : IPipelineConfiguration { }
}
