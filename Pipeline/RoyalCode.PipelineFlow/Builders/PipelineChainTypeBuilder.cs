using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Builders
{
    internal class PipelineChainTypeBuilder<TFor> : PipelineChainTypeBuilder
    {
        public PipelineChainTypeBuilder(
            IPipelineConfiguration<TFor> configuration, 
            IDecoratorSorter decoratorSorter, 
            IEnumerable<IChainTypeBuilder> chainBuilders,
            ChainDelegateRegistry chainDelegateRegistry) 
            : base(configuration, decoratorSorter, chainBuilders, chainDelegateRegistry)
        { }
    }

    internal class PipelineChainTypeBuilder
    {
        private readonly IEnumerable<IChainTypeBuilder> chainBuilders;
        private readonly ChainDelegateRegistry chainDelegateRegistry;
        private readonly HandlerRegistry handlersRegistry;
        private readonly DecoratorRegistry decoratorsRegistry;
        private readonly IDecoratorSorter decoratorSorter;

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

        public Type Build(Type inputType, BridgeChainTypes? bridgeChainTypes = null)
        {
            var handlerDescription = handlersRegistry.GetDescription(inputType);

            if (handlerDescription is null)
                throw new InvalidOperationException($"None handler registrated for type '{inputType.FullName}'.");

            var handlerDelegate = handlerDescription.HandlerDelegateProvider(inputType, typeof(void));
            chainDelegateRegistry.AddDelegate(handlerDelegate);

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
                chainType = chainBuilder.Build(handlerDescription, chainType);
            }
            else
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Handler);
                chainType = chainBuilder.Build(handlerDescription);
            }

            var decoratorDescriptions = decoratorsRegistry.GetDescriptions(inputType);

            if (decoratorDescriptions.Any())
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Decorator);

                decoratorSorter.Sort(decoratorDescriptions)
                    .ToList()
                    .ForEach(decoratorDescription =>
                    {
                        var decoratorDelegate = decoratorDescription.HandlerDelegateProvider(inputType, typeof(void));
                        chainDelegateRegistry.AddDelegate(decoratorDelegate);

                        chainType = chainBuilder.Build(decoratorDescription, chainType);
                    });
            }

            return chainType;
        }

        public Type Build(Type inputType, Type outputType, BridgeChainTypes? bridgeChainTypes = null)
        {
            var handlerDescription = handlersRegistry.GetDescription(inputType, outputType);

            if (handlerDescription is null)
                throw new InvalidOperationException($"None handler registrated for types '{inputType.FullName}' and {outputType.FullName}.");

            var handlerDelegate = handlerDescription.HandlerDelegateProvider(inputType, outputType);
            chainDelegateRegistry.AddDelegate(handlerDelegate);

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
                chainType = chainBuilder.Build(handlerDescription, chainType);
            }
            else
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Handler);
                chainType = chainBuilder.Build(handlerDescription);
            }

            var decoratorDescriptions = decoratorsRegistry.GetDescriptions(inputType, outputType);

            if (decoratorDescriptions.Any())
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Decorator);

                decoratorSorter.Sort(decoratorDescriptions)
                    .ToList()
                    .ForEach(decoratorDescription =>
                    {
                        var decoratorDelegate = decoratorDescription.HandlerDelegateProvider(inputType, outputType);
                        chainDelegateRegistry.AddDelegate(decoratorDelegate);

                        chainType = chainBuilder.Build(decoratorDescription, chainType);
                    });
            }

            return chainType;
        }
    }
}
