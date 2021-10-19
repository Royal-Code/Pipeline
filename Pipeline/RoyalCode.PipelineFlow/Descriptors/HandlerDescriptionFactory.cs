using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// <para>
    ///     Internal utilities for creating handler descriptors.
    /// </para>
    /// <para>
    ///     Processor handlers are produced in this class.
    /// </para>
    /// </summary>
    internal static class HandlerDescriptionFactory
    {
        /// <summary>
        /// Descriptor from a delegate.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        internal static HandlerDescriptor GetHandlerDescription(this Delegate handler)
        {
            var builder = DescriptorBuilder.Create(handler);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            return builder.BuildHandlerDescription();
        }

        /// <summary>
        /// Descriptor from a delegate with a dependent service.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        internal static HandlerDescriptor GetHandlerDescription(this Delegate handler, Type serviceType)
        {
            var builder = DescriptorBuilder.Create(handler, serviceType);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            return builder.BuildHandlerDescription();
        }

        /// <summary>
        /// Descriptor from a method.
        /// </summary>
        /// <param name="handlerMethod"></param>
        /// <returns></returns>
        internal static HandlerDescriptor GetHandlerDescription(this MethodInfo handlerMethod)
        {
            var builder = DescriptorBuilder.Create(handlerMethod);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            builder.ResolveMethodHandlerProvider();
            return builder.BuildHandlerDescription();
        }
    }
}
