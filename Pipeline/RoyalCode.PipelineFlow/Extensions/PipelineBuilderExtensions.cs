using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// Extension methods for <see cref="IPipelineBuilder"/>, 
    /// <see cref="IPipelineBuilder{TIn}"/> and 
    /// <see cref="IPipelineBuilder{TIn, TOut}"/>
    /// </summary>
    public static class PipelineBuilderExtensions
    {
        /// <summary>
        /// Adds many methods as handlers into the pipeline builder.
        /// </summary>
        /// <param name="builder">The pipeline builder for configure.</param>
        /// <param name="methods">The handlers methods</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddHandlerMethods(this IPipelineBuilder builder, params MethodInfo[] methods)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (methods is null)
                throw new ArgumentNullException(nameof(methods));

            foreach (var method in methods)
            {
                builder.AddHandlerResolver(new MethodHandlerResolver(method));
            }

            return builder;
        }

        /// <summary>
        /// Adds all methods from a service to <paramref name="builder"/> as handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="serviceType">The service with handlers methods.</param>
        /// <param name="attributeType">The attribute that define a method as handler.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddHandlerMethodsDefined(
            this IPipelineBuilder builder,
            Type serviceType, 
            Type attributeType)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));

            if (attributeType is null)
                throw new ArgumentNullException(nameof(attributeType));

            var methods = serviceType.GetTypeInfo().GetRuntimeMethods()
                .Where(m => m.IsDefined(attributeType))
                .ToArray();

            return builder.AddHandlerMethods(methods);
        }

        /// <summary>
        /// Add a handler method to <paramref name="builder"/> from a service finding the method by name.
        /// </summary>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="serviceType">The service with one handler method.</param>
        /// <param name="methodName">The name of handler method.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddHandlerMethodDefined(
            this IPipelineBuilder builder,
            Type serviceType,
            string methodName)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));

            if (methodName is null)
                throw new ArgumentNullException(nameof(methodName));

            var methods = serviceType.GetTypeInfo().GetRuntimeMethods()
                .Where(m => m.Name == methodName)
                .ToList();

            if (methods.Count == 0)
                throw new InvalidOperationException(
                    "Error when creating a pipeline handler resolver."
                    + $" Can't find the handler method with name '{methodName}' for service '{serviceType.FullName}'.");

            if (methods.Count > 1)
                throw new InvalidOperationException(
                    "Error when creating a pipeline handler resolver."
                    + $" There are more then one method with name '{methodName}' for service '{serviceType.FullName}'.");

            return builder.AddHandlerMethods(methods[0]);
        }
    }
}
