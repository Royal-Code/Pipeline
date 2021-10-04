using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{

    public interface IHandlerResolver
    {
        bool IsFallback { get; }

        HandlerDescriptor? TryResolve(Type inputType);

        HandlerDescriptor? TryResolve(Type inputType, Type output);
    }
}
