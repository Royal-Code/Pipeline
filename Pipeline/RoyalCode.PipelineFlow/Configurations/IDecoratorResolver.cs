using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IDecoratorResolver
    {
        DecoratorDescription? TryResolve(Type inputType);

        DecoratorDescription? TryResolve(Type inputType, Type output);
    }
}
