using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineBuilder
    {
        void AddHandlerResolver(IHandlerResolver resolver);
    }

    public interface IPipelineBuilder<TIn> : IPipelineBuilder
    {
        IPipelineBuilder<TIn> Handle(Action<TIn> handler);

        IPipelineBuilder<TIn> HandleAsync(Func<TIn, Task> handler);

        IPipelineBuilder<TIn> Handle<TService>(Action<TService, TIn> handler);

        IPipelineBuilder<TIn> HandleAsync<TService>(Func<TService, TIn, Task> handler);
    }

    public interface IPipelineBuilder<TIn, TOut> : IPipelineBuilder
    {
        IPipelineBuilder<TIn, TOut> Handle(Func<TIn, TOut> handler);

        IPipelineBuilder<TIn, TOut> HandleAsync(Func<TIn, Task<TOut>> handler);

        IPipelineBuilder<TIn, TOut> Handle<TService>(Action<TService, TIn, TOut> handler);

        IPipelineBuilder<TIn, TOut> HandleAsync<TService>(Func<TService, TIn, Task<TOut>> handler);
    }

    public class BuildingSample
    {
        public void Buiding(IPipelineBuilder builder)
        {
            IHandlerResolver resolver = null;
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
        IEnumerable<IChainBuilder> chainBuilders; // genérico ou por IPipelineConfigurtion<TFor> ???
        HandlerRegistry handlersRegistry;         // HandlerResolver -> Por IPipelineConfigurtion<TFor>
        DecoratorRegistry decoratorsRegistry;     // DecoratorsResolver -> Por IPipelineConfigurtion<TFor>
        IDecoratorSorter decoratorSorter;         // genérico

        public Type Build(Type inputType)
        {
            var handlerDescription = handlersRegistry.GetDescription(inputType);

            if (handlerDescription is null)
                throw new InvalidOperationException($"None handler registrated for type '{inputType.FullName}'.");

            Type chainType = null;

            if (handlerDescription.IsBridge)
            {
                Type bridgeNextInputType = handlerDescription.GetBridgeType();
                chainType = Build(bridgeNextInputType); // requer algum tratamento para evitar loop infinito

                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Bridge);
                chainType = chainBuilder.Build(handlerDescription, chainType);
            }
            else
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Handler);
                chainType = chainBuilder.Build(handlerDescription);
            }

            var decoratorDescriptions = decoratorsRegistry.GetDescriptions(inputType);
            decoratorDescriptions = decoratorSorter.Sort(decoratorDescriptions);
            
            if (decoratorDescriptions.Any())
            {
                var chainBuilder = chainBuilders.FirstOrDefault(c => c.Kind == ChainKind.Bridge);
                chainType = chainBuilder.Build(handlerDescription, chainType);
            }

            return chainType;
        }
    }

    public enum ChainKind
    {
        Handler,
        Bridge,
        Decorator
    }

    public class HandlerRegistry
    {
        public HandlerDescription GetDescription(Type inputType)
        {
            throw new NotImplementedException();
        }
    }

    public class HandlerDescription
    {
        public Type InputType { get; internal set; }
        public Type OutputType { get; internal set; }
        public bool HasOutput { get; internal set; }
        public bool IsAsync { get; internal set; }
        public bool HasToken { get; internal set; }

        public Delegate HandlerDelegate { get; internal set; }
        public Type ServiceType { get; internal set; }

        public bool IsBridge { get; internal set; }

        public Type GetBridgeType()
        {
            throw new NotImplementedException();
        }
    }

    public class DecoratorRegistry
    {
        public IEnumerable<DecoratorDescription> GetDescriptions(Type inputType)
        {
            throw new NotImplementedException();
        }

    }

    public class DecoratorDescription
    {

    }

    public interface IDecoratorSorter
    {
        IEnumerable<DecoratorDescription> Sort(IEnumerable<DecoratorDescription> descriptions);
    }
}
