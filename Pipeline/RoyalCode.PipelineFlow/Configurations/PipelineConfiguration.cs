namespace RoyalCode.PipelineFlow.Configurations
{
    internal class PipelineConfiguration<TFor> : IPipelineConfiguration<TFor>
    {
        public HandlerRegistry Handlers { get; } = new HandlerRegistry();

        public DecoratorRegistry Decorators { get; } = new DecoratorRegistry();
    }
}
