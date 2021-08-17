using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineBuilder
    {
        void AddHandlerResolver(IHandlerResolver resolver);
    }

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
    }

    public interface IPipelineBuilder<TIn> : IPipelineBuilder
    {
        IPipelineBuilder<TIn> BridgeHandler<TNextInput>(Action<TIn, Action<TNextInput>> handler);
    }

    public class DefaultPipelineBuilder<TIn> : IPipelineBuilder<TIn>
    {
        private readonly IPipelineBuilder pipelineBuilder;

        public DefaultPipelineBuilder(IPipelineBuilder pipelineBuilder)
        {
            this.pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));
        }

        public void AddHandlerResolver(IHandlerResolver resolver) => pipelineBuilder.AddHandlerResolver(resolver);

        #region Bridges

        public IPipelineBuilder<TIn> BridgeHandler<TNextInput>(Action<TIn, Action<TNextInput>> handler)
        {
            pipelineBuilder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler(handler));
            return this;
        }

        #endregion
    }

    public interface IPipelineBuilder<TIn, TOut> : IPipelineBuilder
    {
        IPipelineBuilder<TIn, TOut> Handle(Func<TIn, TOut> handler);

        IPipelineBuilder<TIn, TOut> HandleAsync(Func<TIn, Task<TOut>> handler);

        IPipelineBuilder<TIn, TOut> HandleAsync(Func<TIn, CancellationToken, Task<TOut>> handler);

        IPipelineBuilder<TIn, TOut> Handle<TService>(Func<TService, TIn, TOut> handler);

        IPipelineBuilder<TIn, TOut> HandleAsync<TService>(Func<TService, TIn, Task<TOut>> handler);

        IPipelineBuilder<TIn, TOut> HandleAsync<TService>(Func<TService, TIn, CancellationToken, Task<TOut>> handler);
    }

    public class DefaultPipelineBuilder<TIn, TOut> : IPipelineBuilder<TIn, TOut>
    {
        private readonly IPipelineBuilder pipelineBuilder;

        public DefaultPipelineBuilder(IPipelineBuilder pipelineBuilder)
        {
            this.pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));
        }

        public void AddHandlerResolver(IHandlerResolver resolver) => pipelineBuilder.AddHandlerResolver(resolver);

        public IPipelineBuilder<TIn, TOut> Handle(Func<TIn, TOut> handler)
        {
            pipelineBuilder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return this;
        }

        public IPipelineBuilder<TIn, TOut> Handle<TService>(Func<TService, TIn, TOut> handler)
        {
            pipelineBuilder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return this;
        }

        public IPipelineBuilder<TIn, TOut> HandleAsync(Func<TIn, Task<TOut>> handler)
        {
            pipelineBuilder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return this;
        }

        public IPipelineBuilder<TIn, TOut> HandleAsync(Func<TIn, CancellationToken, Task<TOut>> handler)
        {
            pipelineBuilder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return this;
        }

        public IPipelineBuilder<TIn, TOut> HandleAsync<TService>(Func<TService, TIn, Task<TOut>> handler)
        {
            pipelineBuilder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return this;
        }

        public IPipelineBuilder<TIn, TOut> HandleAsync<TService>(Func<TService, TIn, CancellationToken, Task<TOut>> handler)
        {
            pipelineBuilder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return this;
        }
    }

    public class BuildingSample
    {
        public void Buiding(IPipelineBuilder builder)
        {
            IHandlerResolver resolver = DefaultHandlersResolver
                .Handle<BuildingSample>(sample => Task.FromResult(sample));

            builder.AddHandlerResolver(resolver);
        }

        public void Buiding(IPipelineBuilder<BuildingSample> builder)
        {

        }

        public void Buiding(IPipelineBuilder<BuildingSample, object> builder)
        {

        }
    }

    public class DefaultPipelineChainBuilder
    {
        private readonly IEnumerable<IChainBuilder> chainBuilders;
        private readonly HandlerRegistry handlersRegistry;
        private readonly DecoratorRegistry decoratorsRegistry;
        private readonly IDecoratorSorter decoratorSorter;

        public DefaultPipelineChainBuilder(
            IPipelineConfiguration configuration,
            IDecoratorSorter decoratorSorter,
            IEnumerable<IChainBuilder> chainBuilders)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            handlersRegistry = configuration.Handlers;
            decoratorsRegistry = configuration.Decorators;
            this.decoratorSorter = decoratorSorter ?? throw new ArgumentNullException(nameof(decoratorSorter));
            this.chainBuilders = chainBuilders ?? throw new ArgumentNullException(nameof(chainBuilders));
        }

        public Type Build(Type inputType, BridgeChainTypes? bridgeChainTypes = null)
        {
            var handlerDescription = handlersRegistry.GetDescription(inputType);

            if (handlerDescription is null)
                throw new InvalidOperationException($"None handler registrated for type '{inputType.FullName}'.");

            Type chainType;

            if (handlerDescription.IsBridge)
            {
                if (bridgeChainTypes is null)
                    bridgeChainTypes = new BridgeChainTypes(inputType);

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
