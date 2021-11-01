using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Builders
{
    /// <summary>
    /// This is an internal component used to define the type of the chain class 
    /// from the type of the input and/or output class.
    /// </summary>
    internal class PipelineChainTypeBuilder<TFor> : PipelineChainTypeBuilder, IPipelineChainTypeBuilder<TFor>
    {
        /// <summary>
        /// Creates a new instance with the dependencies.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="decoratorSorter"></param>
        /// <param name="chainBuilders"></param>
        /// <param name="chainDelegateRegistry"></param>
        public PipelineChainTypeBuilder(
            IPipelineConfiguration<TFor> configuration, 
            IDecoratorSorter decoratorSorter, 
            IEnumerable<IChainTypeBuilder> chainBuilders,
            ChainDelegateRegistry chainDelegateRegistry) 
            : base(configuration, decoratorSorter, chainBuilders, chainDelegateRegistry)
        { }
    }

    /// <summary>
    /// This is an internal component used to define the type of the chain class 
    /// from the type of the input and/or output class.
    /// </summary>
    internal class PipelineChainTypeBuilder : IPipelineChainTypeBuilder
    {
        private readonly IEnumerable<IChainTypeBuilder> chainBuilders;
        private readonly ChainDelegateRegistry chainDelegateRegistry;
        private readonly HandlerRegistry handlersRegistry;
        private readonly DecoratorRegistry decoratorsRegistry;
        private readonly IDecoratorSorter decoratorSorter;

        /// <summary>
        /// Creates a new instance with the dependencies.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="decoratorSorter"></param>
        /// <param name="chainBuilders"></param>
        /// <param name="chainDelegateRegistry"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PipelineChainTypeBuilder(
            IPipelineConfiguration configuration,
            IDecoratorSorter decoratorSorter,
            IEnumerable<IChainTypeBuilder> chainBuilders,
            ChainDelegateRegistry chainDelegateRegistry)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            handlersRegistry = configuration.Handlers;
            decoratorsRegistry = configuration.Decorators;
            this.decoratorSorter = decoratorSorter ?? throw new ArgumentNullException(nameof(decoratorSorter));
            this.chainBuilders = chainBuilders ?? throw new ArgumentNullException(nameof(chainBuilders));
            this.chainDelegateRegistry = chainDelegateRegistry ?? throw new ArgumentNullException(nameof(chainDelegateRegistry));
        }

        /// <summary>
        /// Finds out what type of chain class should be used for a pipeline input type.
        /// </summary>
        /// <param name="inputType">The type of the input.</param>
        /// <param name="bridgeChainTypes">Utility for avoiding loops in bridge handlers.</param>
        /// <returns>The type of chain class.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Case it is not possible to create a type because no handler was found or a bridge loop occurred.
        /// </exception>
        public Type Build(Type inputType, BridgeChainTypes? bridgeChainTypes = null)
        {
            var handlerDescription = handlersRegistry.GetDescription(inputType);

            if (handlerDescription is null)
                throw new InvalidOperationException($"None handler registrated for type '{inputType.FullName}'.");

            var handlerDescribed = handlerDescription.Describe(inputType, typeof(void));
            chainDelegateRegistry.AddDelegate(handlerDescribed.Delegate);

            Type chainType;

            if (handlerDescription.IsBridge)
            {
                bridgeChainTypes ??= new BridgeChainTypes(inputType);

                // obtém o tipo do próximo input
                Type bridgeNextInputType = handlerDescription.GetBridgeNextHandlerDescription().InputType;

                // valida o input
                bridgeChainTypes.Enqueue(bridgeNextInputType);

                // gera o chain para o próximo input
                chainType = Build(bridgeNextInputType, bridgeChainTypes);

                // adiciona um chain handler para o bridge.
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Bridge);
                chainType = chainBuilder.Build(handlerDescribed, chainType);
            }
            else
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Handler);
                chainType = chainBuilder.Build(handlerDescribed);
            }

            var decoratorDescriptions = decoratorsRegistry.GetDescriptions(inputType);

            if (decoratorDescriptions.Any())
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Decorator);

                decoratorSorter.Sort(decoratorDescriptions)
                    .ToList()
                    .ForEach(decoratorDescription =>
                    {
                        var decoratorDescribed = decoratorDescription.Describe(inputType, typeof(void));
                        chainDelegateRegistry.AddDelegate(decoratorDescribed.Delegate);

                        chainType = chainBuilder.Build(decoratorDescribed, chainType);
                    });
            }

            return chainType;
        }

        /// <summary>
        /// Finds out what type of chain class should be used for a pipeline input and output types.
        /// </summary>
        /// <param name="inputType">The type of input class.</param>
        /// <param name="outputType">The type of output class.</param>
        /// <param name="bridgeChainTypes">Utility for avoiding loops in bridge handlers.</param>
        /// <returns>The type of chain class.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Case it is not possible to create a type because no handler was found or a bridge loop occurred.
        /// </exception>
        public Type Build(Type inputType, Type outputType, BridgeChainTypes? bridgeChainTypes = null)
        {
            var handlerDescription = handlersRegistry.GetDescription(inputType, outputType);

            if (handlerDescription is null)
                throw new InvalidOperationException($"None handler registrated for types '{inputType.FullName}' and {outputType.FullName}.");

            var handlerDescribed = handlerDescription.Describe(inputType, outputType);
            chainDelegateRegistry.AddDelegate(handlerDescribed.Delegate);

            Type chainType;

            if (handlerDescription.IsBridge)
            {
                bridgeChainTypes ??= new BridgeChainTypes(inputType, outputType);

                // obtém o tipo do próximo input
                var nextHandlerDescription = handlerDescription.GetBridgeNextHandlerDescription();
                Type bridgeNextInputType = nextHandlerDescription.InputType;
                Type bridgeNextOutputType = nextHandlerDescription.OutputType;

                // valida o input
                bridgeChainTypes.Enqueue(bridgeNextInputType, bridgeNextOutputType);

                // gera o chain para o próximo input
                chainType = Build(bridgeNextInputType, bridgeNextOutputType, bridgeChainTypes);

                // adiciona um chain handler para o bridge.
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Bridge);
                chainType = chainBuilder.Build(handlerDescribed, chainType);
            }
            else
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Handler);
                chainType = chainBuilder.Build(handlerDescribed);
            }

            var decoratorDescriptions = decoratorsRegistry.GetDescriptions(inputType, outputType);

            if (decoratorDescriptions.Any())
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Decorator);

                decoratorSorter.Sort(decoratorDescriptions)
                    .ToList()
                    .ForEach(decoratorDescription =>
                    {
                        var decoratorDescribed = decoratorDescription.Describe(inputType, outputType);
                        chainDelegateRegistry.AddDelegate(decoratorDescribed.Delegate);

                        chainType = chainBuilder.Build(decoratorDescribed, chainType);
                    });
            }

            return chainType;
        }
    }
}
