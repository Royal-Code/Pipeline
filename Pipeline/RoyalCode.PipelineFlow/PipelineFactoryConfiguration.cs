using RoyalCode.PipelineFlow.Builders;
using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow
{
    public class PipelineFactoryConfiguration<TFor>
    {
        private readonly ChainDelegateRegistry chainDelegateRegistry = new();
        private IServiceProvider? userServiceProvider = null;

        internal PipelineFactoryConfiguration() 
        {
            ServiceFactoryCollection
                .AddServiceFactory<ChainDelegateRegistry>((type, sp) => chainDelegateRegistry)
                .MapService(typeof(IChainDelegateProvider<>), typeof(ChainDelegateProvider<>));
        }

        /// <summary>
        /// The chains configurations.
        /// </summary>
        public IPipelineConfiguration<TFor> Configuration { get; } = new PipelineConfiguration<TFor>();
        
        /// <summary>
        /// The builder of the chains, with the default buiders.
        /// </summary>
        public ICollection<IChainTypeBuilder> ChainBuilders { get; } = new List<IChainTypeBuilder>()
        {
            new HandlerChainTypeBuilder(),
            new BridgeChainTypeBuilder(),
            new DecoratorChainTypeBuilder(),
        };

        /// <summary>
        /// <para>
        ///     Collection of factories for services.
        /// </para>
        /// <para>
        ///     This collection will be ignored if the <see cref="PipelineFactoryConfiguration{TFor}.ServiceProvider"/>
        ///     has being configured.
        /// </para>
        /// </summary>
        public ServiceFactoryCollection ServiceFactoryCollection { get; } = new ServiceFactoryCollection();

        /// <summary>
        /// <para>
        ///     The service provider for resolve the services and chains dependencies.
        /// </para>
        /// </summary>
        public IServiceProvider ServiceProvider 
        {
            set => userServiceProvider = value;
        }

        /// <summary>
        /// Create the pipeline factory for some kind of component.
        /// </summary>
        /// <returns>A new instance of <see cref="IPipelineFactory{TFor}"/>.</returns>
        public IPipelineFactory<TFor> Create()
        {
            var chainPipelineBuilder = new PipelineChainTypeBuilder<TFor>(
                Configuration, new DecoratorSorter(), ChainBuilders, chainDelegateRegistry);

            var pipelineTypeBuilder = new PipelineTypeBuilder(userServiceProvider ?? ServiceFactoryCollection.BuildServiceProvider());

            return new PipelineFactory<TFor>(chainPipelineBuilder, pipelineTypeBuilder);
        }

        public PipelineFactoryConfiguration<TFor> ConfigurePipelines(Action<IPipelineBuilder> configurer)
        {
            if (configurer is null)
                throw new ArgumentNullException(nameof(configurer));

            configurer(new DefaultPipelineBuilder(Configuration));

            return this;
        }
    }
}
