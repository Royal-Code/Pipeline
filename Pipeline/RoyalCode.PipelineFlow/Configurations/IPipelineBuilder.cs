﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineBuilder
    {
        void AddHandlerResolver(IHandlerResolver resolver);

        void AddDecoratorResolver(IDecoratorResolver resolver);

        IPipelineBuilder<TIn> Configure<TIn>();

        IPipelineBuilder<TIn, TOut> Configure<TIn, TOut>();
    }

    public interface IPipelineBuilder<TIn> : IPipelineBuilder
    {
        IPipelineBuilderWithService<TService, TIn> WithService<TService>();
    }

    public interface IPipelineBuilder<TIn, TOut> : IPipelineBuilder
    {
        IPipelineBuilderWithService<TService, TIn, TOut> WithService<TService>();
    }

    public interface IPipelineBuilderWithService<TService> : IPipelineBuilder { }

    public interface IPipelineBuilderWithService<TService, TIn> : IPipelineBuilderWithService<TService> { }

    public interface IPipelineBuilderWithService<TService, TIn, TOut> : IPipelineBuilderWithService<TService> { }

    public class DeafultPipelineBuilder : IPipelineBuilder
    {
        private readonly IPipelineConfiguration configuration;

        public DeafultPipelineBuilder(IPipelineConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void AddHandlerResolver(IHandlerResolver resolver)
        {
            configuration.Handlers.Add(resolver);
        }

        public void AddDecoratorResolver(IDecoratorResolver resolver)
        {
            configuration.Decorators.Add(resolver);
        }

        public IPipelineBuilder<TIn> Configure<TIn>() => new DefaultPipelineBuilder<TIn>(this);

        public IPipelineBuilder<TIn, TOut> Configure<TIn, TOut>() => new DefaultPipelineBuilder<TIn, TOut>(this);
    }

    public abstract class PipelineBuilderBase : IPipelineBuilder
    {
        protected readonly IPipelineBuilder pipelineBuilder;

        internal protected PipelineBuilderBase(IPipelineBuilder pipelineBuilder)
        {
            this.pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));
        }

        public void AddHandlerResolver(IHandlerResolver resolver) => pipelineBuilder.AddHandlerResolver(resolver);

        public void AddDecoratorResolver(IDecoratorResolver resolver) => pipelineBuilder.AddDecoratorResolver(resolver);

        public IPipelineBuilder<TInput> Configure<TInput>() => pipelineBuilder.Configure<TInput>();

        public IPipelineBuilder<TInput, TOut> Configure<TInput, TOut>() => pipelineBuilder.Configure<TInput, TOut>();
    }

    public class DefaultPipelineBuilderWithService<TService> : PipelineBuilderBase, IPipelineBuilderWithService<TService>
    {
        public DefaultPipelineBuilderWithService(IPipelineBuilder builder)
            : base(builder)
        { }
    }

    public class DefaultPipelineBuilder<TIn> : PipelineBuilderBase, IPipelineBuilder<TIn>
    {
        public DefaultPipelineBuilder(IPipelineBuilder pipelineBuilder)
            : base(pipelineBuilder)
        { }

        public IPipelineBuilderWithService<TService, TIn> WithService<TService>() => new DefaultPipelineBuilderWithService<TService, TIn>(pipelineBuilder);
    }

    public class DefaultPipelineBuilderWithService<TService, TIn> : DefaultPipelineBuilder<TIn>, IPipelineBuilderWithService<TService, TIn>
    {
        public DefaultPipelineBuilderWithService(IPipelineBuilder pipelineBuilder) : base(pipelineBuilder) { }
    }

    public class DefaultPipelineBuilder<TIn, TOut> : PipelineBuilderBase, IPipelineBuilder<TIn, TOut>
    {
        public DefaultPipelineBuilder(IPipelineBuilder pipelineBuilder) : base(pipelineBuilder) { }

        public IPipelineBuilderWithService<TService, TIn, TOut> WithService<TService>() => new DefaultPipelineBuilderWithService<TService, TIn, TOut>(pipelineBuilder);
    }

    public class DefaultPipelineBuilderWithService<TService, TIn, TOut> : DefaultPipelineBuilder<TIn, TOut>, IPipelineBuilderWithService<TService, TIn, TOut>
    {
        public DefaultPipelineBuilderWithService(IPipelineBuilder pipelineBuilder) : base(pipelineBuilder) { }
    }

    internal class DefaultPipelineChainBuilder
    {
        private readonly IEnumerable<IChainTypeBuilder> chainBuilders;
        private readonly ChainDelegateRegistry chainDelegateRegistry;
        private readonly HandlerRegistry handlersRegistry;
        private readonly DecoratorRegistry decoratorsRegistry;
        private readonly IDecoratorSorter decoratorSorter;

        public DefaultPipelineChainBuilder(
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
                        chainType = chainBuilder.Build(decoratorDescription, chainType);
                    });
            }

            return chainType;
        }
    }



    public interface IDecoratorSorter
    {
        IEnumerable<DecoratorDescription> Sort(IEnumerable<DecoratorDescription> descriptions);
    }
}
