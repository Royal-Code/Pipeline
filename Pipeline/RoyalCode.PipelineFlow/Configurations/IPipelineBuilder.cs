using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineBuilder
    {

    }

    public interface IPipelineBuilder<TIn> : IPipelineBuilder
    {
    }

    public interface IPipelineBuilder<TIn, TOut> : IPipelineBuilder
    {
    }

    public class DefaultPipelineBuilder : IPipelineBuilder
    {
        IEnumerable<IChainBuilder> chainBuilders; // genérico ou por IPipelineConfigurtion<TFor> ???
        HandlersRegistry handlersRegistry;        // HandlerResolver -> Por IPipelineConfigurtion<TFor>
        DecoratorsRegistry decoratorsRegistry;    // DecoratorsResolver -> Por IPipelineConfigurtion<TFor>
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

    public class HandlersRegistry
    {
        public HandlerDescription GetDescription(Type inputType)
        {
            throw new NotImplementedException();
        }
    }

    public class HandlerDescription
    {
        public bool IsBridge { get; internal set; }

        public Type GetBridgeType()
        {
            throw new NotImplementedException();
        }
    }

    public class DecoratorsRegistry
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
