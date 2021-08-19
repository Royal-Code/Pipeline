using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// Extension methods for <see cref="IPipelineBuilder"/>, 
    /// <see cref="IPipelineBuilder{TIn}"/> and 
    /// <see cref="IPipelineBuilder{TIn, TOut}"/>
    /// </summary>
    public static class PipelineBuilderExtensions
    {
        #region PipelineBuilder

        public static IPipelineBuilderWithService<TService> ConfigureWithService<TService>(this IPipelineBuilder pipelineBuilder)
        {
            return new DefaultPipelineBuilderWithService<TService>(pipelineBuilder);
        }

        #endregion

        #region Handlers TIn

        public static IPipelineBuilder<TIn> Handle<TIn>(this IPipelineBuilder<TIn> builder, Action<TIn> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> Handle<TService, TIn>(this IPipelineBuilderWithService<TService, TIn> builder, Action<TService, TIn> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder.Configure<TIn>();
        }

        public static IPipelineBuilder<TIn> HandleAsync<TIn>(this IPipelineBuilder<TIn> builder, Func<TIn, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> HandleAsync<TIn>(this IPipelineBuilder<TIn> builder, Func<TIn, CancellationToken, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> HandleAsync<TService, TIn>(this IPipelineBuilderWithService<TService, TIn> builder, Func<TService, TIn, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder.Configure<TIn>();
        }

        public static IPipelineBuilder<TIn> HandleAsync<TService, TIn>(this IPipelineBuilderWithService<TService, TIn> builder, Func<TService, TIn, CancellationToken, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder.Configure<TIn>();
        }

        #endregion

        #region Handlers TIn TOut

        public static IPipelineBuilder<TIn, TOut> Handle<TIn, TOut>(this IPipelineBuilder<TIn, TOut> builder, Func<TIn, TOut> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> Handle<TService, TIn, TOut>(this IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, TOut> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> HandleAsync<TIn, TOut>(this IPipelineBuilder<TIn, TOut> builder, Func<TIn, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> HandleAsync<TIn, TOut>(this IPipelineBuilder<TIn, TOut> builder, Func<TIn, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> HandleAsync<TService, TIn, TOut>(this IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> HandleAsync<TService, TIn, TOut>(this IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.Handle(handler));
            return builder.Configure<TIn, TOut>();
        }

        #endregion

        #region Method Handlers

        /// <summary>
        /// Add a method as a handler into pipeline builder.
        /// </summary>
        /// <param name="builder">The pipeline builder for configure.</param>
        /// <param name="method">The handler method.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder Handle(this IPipelineBuilder builder, MethodInfo method)
        {
            builder.AddHandlerResolver(new MethodHandlerResolver(method));
            return builder;
        }

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
        /// Adds all methods from a service to <paramref name="builder"/> as handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <typeparam name="TService">The service with handlers methods.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="attributeType">The attribute that define a method as handler.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddHandlerMethodsDefined<TService>(
            this IPipelineBuilderWithService<TService> builder,
            Type attributeType)
        {
            return builder.AddHandlerMethodsDefined(typeof(TService), attributeType);
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

        /// <summary>
        /// Add a handler method to <paramref name="builder"/> from a service finding the method by name.
        /// </summary>
        /// <typeparam name="TService">The service with one handler method.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="methodName">The name of handler method.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddHandlerMethodDefined<TService>(
            this IPipelineBuilder<TService> builder,
            string methodName)
        {
            return builder.AddHandlerMethodDefined(typeof(TService), methodName);
        }

        #endregion

        #region Bridges Handlers TIn

        public static IPipelineBuilder<TIn> BridgeHandle<TIn, TNextInput>(IPipelineBuilder<TIn> builder, Action<TIn, Action<TNextInput>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> BridgeHandleAsync<TIn, TNextInput>(IPipelineBuilder<TIn> builder, Func<TIn, Func<TNextInput, Task>, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> BridgeHandleAsync<TIn, TNextInput>(IPipelineBuilder<TIn> builder, Func<TIn, Func<TNextInput, Task>, CancellationToken, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> BridgeHandle<TService, TIn, TNextInput>(IPipelineBuilderWithService<TService, TIn> builder, Action<TService, TIn, Action<TNextInput>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler(handler));
            return builder.Configure<TIn>();
        }

        public static IPipelineBuilder<TIn> BridgeHandleAsync<TService, TIn, TNextInput>(IPipelineBuilderWithService<TService, TIn> builder, Func<TService, TIn, Func<TNextInput, Task>, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler(handler));
            return builder.Configure<TIn>();
        }

        public static IPipelineBuilder<TIn> BridgeHandleAsync<TService, TIn, TNextInput>(IPipelineBuilderWithService<TService, TIn> builder, Func<TService, TIn, Func<TNextInput, Task>, CancellationToken, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler(handler));
            return builder.Configure<TIn>();
        }

        #endregion

        #region Bridges Handlers TIn TOut

        public static IPipelineBuilder<TIn, TOut> BridgeHandle<TIn, TOut, TNextInput>(IPipelineBuilder<TIn, TOut> builder, Func<TIn, Func<TNextInput, TOut>, TOut> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TIn, TOut, TNextInput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TIn, TOut, TNextInput>(IPipelineBuilder<TIn, TOut> builder, Func<TIn, Func<TNextInput, Task<TOut>>, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TIn, TOut, TNextInput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TIn, TOut, TNextInput>(IPipelineBuilder<TIn, TOut> builder, Func<TIn, Func<TNextInput, Task<TOut>>, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TIn, TOut, TNextInput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandle<TService, TIn, TOut, TNextInput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, TOut>, TOut> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TService, TIn, TOut, TNextInput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TService, TIn, TOut, TNextInput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, Task<TOut>>, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TService, TIn, TOut, TNextInput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TService, TIn, TOut, TNextInput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, Task<TOut>>, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TService, TIn, TOut, TNextInput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        #endregion

        #region Bridges Handlers TIn TOut, TNextOutput

        public static IPipelineBuilder<TIn, TOut> BridgeHandle<TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilder<TIn, TOut> builder, Func<TIn, Func<TNextInput, TNextOutput>, TOut> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilder<TIn, TOut> builder, Func<TIn, Func<TNextInput, Task<TNextOutput>>, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilder<TIn, TOut> builder, Func<TIn, Func<TNextInput, Task<TNextOutput>>, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandle<TService, TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, TNextOutput>, TOut> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TService, TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TService, TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, Task<TNextOutput>>, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TService, TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TService, TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, Task<TNextOutput>>, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TService, TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        #endregion

        #region Method Bridges Handlers

        #endregion

        #region Decorators Handlers TIn

        #endregion

        #region Decorators Handlers TIn TOut

        #endregion

        #region Method Decorators Handlers

        #endregion
    }
}
