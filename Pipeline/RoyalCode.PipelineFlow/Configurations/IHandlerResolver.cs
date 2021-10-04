using System;

namespace RoyalCode.PipelineFlow.Configurations
{

    public interface IHandlerResolver
    {
        bool IsFallback { get; }

        HandlerDescription? TryResolve(Type inputType);

        HandlerDescription? TryResolve(Type inputType, Type output);
    }
}
