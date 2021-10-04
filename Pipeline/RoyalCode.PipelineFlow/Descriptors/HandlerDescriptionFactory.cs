using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal static class HandlerDescriptionFactory
    {
        internal static HandlerDescription GetHandlerDescription(this Delegate handler)
        {
            var builder = DescriptorBuilder.Create(handler);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            return builder.BuildHandlerDescription();
        }

        internal static HandlerDescription GetHandlerDescription(this Delegate handler, Type serviceType)
        {
            var builder = DescriptorBuilder.Create(handler, serviceType);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            return builder.BuildHandlerDescription();
        }

        internal static HandlerDescription GetHandlerDescription(this MethodInfo handlerMethod)
        {
            var builder = DescriptorBuilder.Create(handlerMethod);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            builder.ResolveMethodHandlerProvider();
            return builder.BuildHandlerDescription();
        }
    }
}
