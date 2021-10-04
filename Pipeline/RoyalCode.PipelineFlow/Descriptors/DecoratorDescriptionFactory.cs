using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Descriptors
{
    internal static class DecoratorDescriptionFactory
    {
        internal static DecoratorDescriptor GetDecoratorDescription(this Delegate decoratorHandler)
        {
            var builder = DescriptorBuilder.Create(decoratorHandler);
            builder.ReadDecoratorParameters();
            builder.ValidateDecoratorParameters();
            return builder.BuildDecoratorDescription();
        }

        internal static DecoratorDescriptor GetDecoratorDescription(this Delegate decoratorHandler, Type serviceType)
        {
            var builder = DescriptorBuilder.Create(decoratorHandler, serviceType);
            builder.ReadDecoratorParameters();
            builder.ValidateDecoratorParameters();
            return builder.BuildDecoratorDescription();
        }

        internal static DecoratorDescriptor GetDecoratorDescription(this MethodInfo decoratorMethod)
        {
            var builder = DescriptorBuilder.Create(decoratorMethod);
            builder.ReadDecoratorParameters();
            builder.ValidateDecoratorParameters();
            builder.ResolveMethodHandlerProvider();
            return builder.BuildDecoratorDescription();
        }
    }
}
