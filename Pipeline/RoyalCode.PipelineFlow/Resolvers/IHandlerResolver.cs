using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{

    public interface IHandlerResolver
    {
        bool IsFallback { get; }

        HandlerDescriptor? TryResolve(Type inputType);

        HandlerDescriptor? TryResolve(Type inputType, Type output);
    }
}
