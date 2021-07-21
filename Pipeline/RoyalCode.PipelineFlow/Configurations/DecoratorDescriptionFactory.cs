using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal static class DecoratorDescriptionFactory
    {
        internal static DecoratorDescription GetDecoratorDescription(this Delegate decoratorHandler)
        {
            var builder = DescriptionBuilder.Create(decoratorHandler);
            builder.ReadDecoratorParameters();
            builder.ValidateDecoratorParameters();
            return builder.BuildDecoratorDescription();
        }

        internal static DecoratorDescription GetDecoratorDescription(this Delegate decoratorHandler, Type serviceType)
        {
            var builder = DescriptionBuilder.Create(decoratorHandler, serviceType);
            builder.ReadDecoratorParameters();
            builder.ValidateDecoratorParameters();
            return builder.BuildDecoratorDescription();
        }

        internal static DecoratorDescription GetDecoratorDescription(this MethodInfo decoratorMethod)
        {
            var builder = DescriptionBuilder.Create(decoratorMethod);
            builder.ReadDecoratorParameters();
            builder.ValidateDecoratorParameters();
            builder.ResolveMethodHandlerProvider();
            return builder.BuildDecoratorDescription();
        }
    }
}
