using RoyalCode.PipelineFlow.Builders;
using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow
{
    public class PipelineFactoryConfiguration<TFor>
    {
        private IPipelineTypeBuilder? pipelineTypeBuilder = null;
        private DecoratorSorter decoratorSorter = new();
        private ChainDelegateRegistry chainDelegateRegistry = new();

        internal PipelineFactoryConfiguration() { }

        public IPipelineConfiguration<TFor> Configuration { get; } = new PipelineConfiguration<TFor>();

        public ICollection<IChainTypeBuilder> ChainBuilders { get; } = new List<IChainTypeBuilder>()
        {
            new HandlerChainTypeBuilder(),
            new BridgeChainTypeBuilder(),
            new DecoratorChainTypeBuilder(),
        };

        public IPipelineFactory<TFor> Create()
        {
            var chainPipelineBuilder = new PipelineChainTypeBuilder<TFor>(
                Configuration, decoratorSorter, ChainBuilders, chainDelegateRegistry);

            return new PipelineFactory<TFor>(chainPipelineBuilder, pipelineTypeBuilder);
        }
    }

    internal class PipelineTypeBuilder : IPipelineTypeBuilder
    {


        public object Build(Type chainType)
        {
            throw new NotImplementedException();
        }
    }
}
