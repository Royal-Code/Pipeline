using RoyalCode.PipelineFlow.Builders;
using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow
{
    /// <summary>
    /// The configuration for build a <see cref="IPipelineFactory{TFor}"/>.
    /// </summary>
    /// <typeparam name="TFor">The specific type of the pipeline.</typeparam>
    public class PipelineFactoryConfiguration<TFor>
    {
        private readonly ChainDelegateRegistry chainDelegateRegistry;
        private IServiceProvider? userServiceProvider = null;

        internal PipelineFactoryConfiguration(ChainDelegateRegistry chainDelegateRegistry) 
        {
            this.chainDelegateRegistry = chainDelegateRegistry;

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
        /// 
        /// </summary>
        /// <returns></returns>
        public IPipelineChainTypeBuilder<TFor> CreatePipelineChainTypeBuilder()
        {
            return new PipelineChainTypeBuilder<TFor>(
                Configuration, 
                new DecoratorSorter(), 
                ChainBuilders, 
                chainDelegateRegistry);
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

            return new PipelineFactory<TFor>(CreatePipelineChainTypeBuilder(), pipelineTypeBuilder);
        }

        /// <summary>
        /// Configure handlers for the pipeline.
        /// </summary>
        /// <param name="configurer">An action to configure the handlers for the pipeline.</param>
        /// <returns>The same instance for chain calls.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="configurer"/> is null.
        /// </exception>
        public PipelineFactoryConfiguration<TFor> ConfigurePipelines(Action<IPipelineBuilder> configurer)
        {
            if (configurer is null)
                throw new ArgumentNullException(nameof(configurer));

            configurer(new DefaultPipelineBuilder(Configuration));

            return this;
        }
    }
}
