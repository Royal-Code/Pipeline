using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal static class HandlerDescriptionFactory
    {
        internal static HandlerDescription GetHandlerDescription(this Delegate handler)
        {
            var builder = DescriptionBuilder.Create(handler);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            return builder.BuildHandlerDescription();
        }

        internal static HandlerDescription GetHandlerDescription(this Delegate handler, Type serviceType)
        {
            var builder = DescriptionBuilder.Create(handler, serviceType);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            return builder.BuildHandlerDescription();
        }

        internal static HandlerDescription GetHandlerDescription(this MethodInfo handlerMethod)
        {
            var builder = DescriptionBuilder.Create(handlerMethod);
            builder.ReadHandlerParameters();
            builder.ValidateHandlerParameters();
            builder.ResolveMethodHandlerProvier();
            return builder.BuildHandlerDescription();
        }
    }
}
