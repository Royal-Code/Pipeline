using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineConfiguration
    {
        HandlerRegistry Handlers { get; }

        DecoratorRegistry Decorators { get; }
    }

    public interface IPipelineConfiguration<TFor> : IPipelineConfiguration
    {

    }

    public interface IChainBuilder
    {
        ChainKind Kind { get; }

        Type Build(DecoratorDescription decoratorDescription, Type previousChainType);
        Type Build(HandlerDescription handlerDescription, Type previousChainType);
        Type Build(HandlerDescription handlerDescription);


    }
}
