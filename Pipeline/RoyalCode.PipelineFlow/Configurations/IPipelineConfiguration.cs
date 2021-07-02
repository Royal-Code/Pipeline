using System;
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

        Type Build(HandlerDescription handlerDescription, Type previousChainType);
        Type Build(HandlerDescription handlerDescription);
    }

    public interface IHandlerResolver
    {
        HandlerDescription TryResolve(Type inputType);

        HandlerDescription TryResolve(Type inputType, Type output);
    }

    public class DelegateHandlerResolver : IHandlerResolver
    {
        private readonly HandlerDescription handlerDescription;

        public DelegateHandlerResolver(Delegate handler)
        {
            handlerDescription = handler.GetHandlerDescription();
        }

        public HandlerDescription TryResolve(Type inputType, Type output)
        {
            return handlerDescription.InputType == inputType && handlerDescription.OutputType == output
                ? handlerDescription
                : null;
        }

        HandlerDescription IHandlerResolver.TryResolve(Type inputType)
        {
            return handlerDescription.InputType == inputType && !handlerDescription.HasOutput 
                ? handlerDescription 
                : null;
        }
    }

    public class ServiceAndDelegateHandlerResolver : IHandlerResolver
    {
        private readonly HandlerDescription handlerDescription;

        public ServiceAndDelegateHandlerResolver(Delegate handler, Type serviceType)
        {
            handlerDescription = handler.GetHandlerDescription(serviceType);
        }

        public HandlerDescription TryResolve(Type inputType)
        {
            return handlerDescription.Match(inputType)
                ? handlerDescription
                : null;
        }

        public HandlerDescription TryResolve(Type inputType, Type output)
        {
            return handlerDescription.Match(inputType, output)
                ? handlerDescription
                : null;
        }
    }
}
