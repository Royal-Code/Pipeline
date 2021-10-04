using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    public interface IDecoratorResolver
    {
        DecoratorDescriptor? TryResolve(Type inputType);

        DecoratorDescriptor? TryResolve(Type inputType, Type output);
    }
}
