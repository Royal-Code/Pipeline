using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Descriptors
{
    internal static class HandlerDescriptionFactory
    {
        internal static HandlerDescriptor GetHandlerDescription(this Delegate handler)
        {
            var builder = DescriptorBuilder.Create(handler);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            return builder.BuildHandlerDescription();
        }

        internal static HandlerDescriptor GetHandlerDescription(this Delegate handler, Type serviceType)
        {
            var builder = DescriptorBuilder.Create(handler, serviceType);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            return builder.BuildHandlerDescription();
        }

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
