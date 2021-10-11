using RoyalCode.PipelineFlow.Builders;
using System;

namespace RoyalCode.PipelineFlow
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
