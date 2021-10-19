using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// <para>
    ///     Internal utilities for creating handler descriptors.
    /// </para>
    /// <para>
    ///     Decorators handlers are produced in this class.
    /// </para>
    /// </summary>
    internal static class DecoratorDescriptionFactory
    {
        /// <summary>
        /// Descriptor from a delegate.
        /// </summary>
        /// <param name="decoratorHandler"></param>
        /// <returns></returns>
        internal static DecoratorDescriptor GetDecoratorDescription(this Delegate decoratorHandler)
        {
            var builder = DescriptorBuilder.Create(decoratorHandler);
            builder.ReadDecoratorParameters();
            builder.ValidateDecoratorParameters();
            return builder.BuildDecoratorDescription();
        }

        /// <summary>
        /// Descriptor from a delegate with a dependent service.
        /// </summary>
        /// <param name="decoratorHandler"></param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        internal static DecoratorDescriptor GetDecoratorDescription(this Delegate decoratorHandler, Type serviceType)
        {
            var builder = DescriptorBuilder.Create(decoratorHandler, serviceType);
            builder.ReadDecoratorParameters();
            builder.ValidateDecoratorParameters();
            return builder.BuildDecoratorDescription();
        }

        /// <summary>
        /// Descriptor from a method.
        /// </summary>
        /// <param name="decoratorMethod"></param>
        /// <returns></returns>
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
