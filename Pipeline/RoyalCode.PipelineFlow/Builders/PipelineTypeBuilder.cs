using System;

namespace RoyalCode.PipelineFlow.Builders
{
    internal class PipelineTypeBuilder : IPipelineTypeBuilder
    {
        private readonly IServiceProvider provider;

        public PipelineTypeBuilder(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public object Build(Type chainType) => provider.GetService(chainType);
    }
}
