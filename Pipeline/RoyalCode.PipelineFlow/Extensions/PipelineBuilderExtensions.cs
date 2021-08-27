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
            builder.AddHandlerResolver(DefaultHandlersResolver.HandleAsync(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> HandleAsync<TIn>(this IPipelineBuilder<TIn> builder, Func<TIn, CancellationToken, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.HandleAsync(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> HandleAsync<TService, TIn>(this IPipelineBuilderWithService<TService, TIn> builder, Func<TService, TIn, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.HandleAsync(handler));
            return builder.Configure<TIn>();
        }

        public static IPipelineBuilder<TIn> HandleAsync<TService, TIn>(this IPipelineBuilderWithService<TService, TIn> builder, Func<TService, TIn, CancellationToken, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.HandleAsync(handler));
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
            builder.AddHandlerResolver(DefaultHandlersResolver.HandleAsync(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> HandleAsync<TIn, TOut>(this IPipelineBuilder<TIn, TOut> builder, Func<TIn, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.HandleAsync(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> HandleAsync<TService, TIn, TOut>(this IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.HandleAsync(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> HandleAsync<TService, TIn, TOut>(this IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.HandleAsync(handler));
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
        public static IPipelineBuilder AddHandlersMethods(this IPipelineBuilder builder, params MethodInfo[] methods)
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
        public static IPipelineBuilder AddHandlersMethodsDefined(
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

            return builder.AddHandlersMethods(methods);
        }

        /// <summary>
        /// Adds all methods from a service to <paramref name="builder"/> as handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <typeparam name="TService">The service with handlers methods.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="attributeType">The attribute that define a method as handler.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddHandlersMethodsDefined<TService>(
            this IPipelineBuilderWithService<TService> builder,
            Type attributeType)
        {
            return builder.AddHandlersMethodsDefined(typeof(TService), attributeType);
        }

        /// <summary>
        /// Adds all methods from a service to <paramref name="builder"/> as handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <typeparam name="TService">The service with handlers methods.</typeparam>
        /// <typeparam name="TAttribute">The attribute that define a method as handler.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddHandlersMethodsDefined<TService, TAttribute>(
            this IPipelineBuilderWithService<TService> builder)
            where TAttribute: Attribute
        {
            return builder.AddHandlersMethodsDefined(typeof(TService), typeof(TAttribute));
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

            return builder.Handle(methods[0]);
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
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> BridgeHandleAsync<TIn, TNextInput>(IPipelineBuilder<TIn> builder, Func<TIn, Func<TNextInput, Task>, CancellationToken, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn> BridgeHandle<TService, TIn, TNextInput>(IPipelineBuilderWithService<TService, TIn> builder, Action<TService, TIn, Action<TNextInput>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler(handler));
            return builder.Configure<TIn>();
        }

        public static IPipelineBuilder<TIn> BridgeHandleAsync<TService, TIn, TNextInput>(IPipelineBuilderWithService<TService, TIn> builder, Func<TService, TIn, Func<TNextInput, Task>, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync(handler));
            return builder.Configure<TIn>();
        }

        public static IPipelineBuilder<TIn> BridgeHandleAsync<TService, TIn, TNextInput>(IPipelineBuilderWithService<TService, TIn> builder, Func<TService, TIn, Func<TNextInput, Task>, CancellationToken, Task> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync(handler));
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
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync<TIn, TOut, TNextInput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TIn, TOut, TNextInput>(IPipelineBuilder<TIn, TOut> builder, Func<TIn, Func<TNextInput, Task<TOut>>, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync<TIn, TOut, TNextInput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandle<TService, TIn, TOut, TNextInput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, TOut>, TOut> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TService, TIn, TOut, TNextInput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TService, TIn, TOut, TNextInput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, Task<TOut>>, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync<TService, TIn, TOut, TNextInput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TService, TIn, TOut, TNextInput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, Task<TOut>>, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync<TService, TIn, TOut, TNextInput>(handler));
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
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync<TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilder<TIn, TOut> builder, Func<TIn, Func<TNextInput, Task<TNextOutput>>, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync<TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder;
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandle<TService, TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, TNextOutput>, TOut> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandler<TService, TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TService, TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, Task<TNextOutput>>, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync<TService, TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        public static IPipelineBuilder<TIn, TOut> BridgeHandleAsync<TService, TIn, TOut, TNextInput, TNextOutput>(IPipelineBuilderWithService<TService, TIn, TOut> builder, Func<TService, TIn, Func<TNextInput, Task<TNextOutput>>, CancellationToken, Task<TOut>> handler)
        {
            builder.AddHandlerResolver(DefaultHandlersResolver.BridgeHandlerAsync<TService, TIn, TOut, TNextInput, TNextOutput>(handler));
            return builder.Configure<TIn, TOut>();
        }

        #endregion

        #region Method Bridges Handlers

        /// <summary>
        /// Add a method as a bridge handler into pipeline builder.
        /// </summary>
        /// <param name="builder">The pipeline builder for configure.</param>
        /// <param name="method">The bridge handler method.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder BridgeHandle(this IPipelineBuilder builder, MethodInfo method)
        {
            builder.AddHandlerResolver(new MethodBridgeHandlerResolver(method));
            return builder;
        }

        /// <summary>
        /// Adds many methods as bridge handlers into the pipeline builder.
        /// </summary>
        /// <param name="builder">The pipeline builder for configure.</param>
        /// <param name="methods">The bridge handlers methods</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddBridgeHandlersMethods(this IPipelineBuilder builder, params MethodInfo[] methods)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (methods is null)
                throw new ArgumentNullException(nameof(methods));

            foreach (var method in methods)
            {
                builder.AddHandlerResolver(new MethodBridgeHandlerResolver(method));
            }

            return builder;
        }

        /// <summary>
        /// Adds all methods from a service to <paramref name="builder"/> as bridge handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="serviceType">The service with bridge handlers methods.</param>
        /// <param name="attributeType">The attribute that define a method as bridge handler.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddBridgeHandlersMethodsDefined(
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

            return builder.AddBridgeHandlersMethods(methods);
        }

        /// <summary>
        /// Adds all methods from a service to <paramref name="builder"/> as bridge handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <typeparam name="TService">The service with bridge handlers methods.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="attributeType">The attribute that define a method as bridge handler.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddBridgeHandlersMethodsDefined<TService>(
            this IPipelineBuilderWithService<TService> builder,
            Type attributeType)
        {
            return builder.AddBridgeHandlersMethodsDefined(typeof(TService), attributeType);
        }

        /// <summary>
        /// Adds all methods from a service to <paramref name="builder"/> as bridge handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <typeparam name="TService">The service with bridge handlers methods.</typeparam>
        /// <typeparam name="TAttribute">The attribute that define a method as bridge handler.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddBridgeHandlersMethodsDefined<TService, TAttribute>(
            this IPipelineBuilderWithService<TService> builder)
            where TAttribute : Attribute
        {
            return builder.AddBridgeHandlersMethodsDefined(typeof(TService), typeof(TAttribute));
        }

        /// <summary>
        /// Add a bridge handler method to <paramref name="builder"/> from a service finding the method by name.
        /// </summary>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="serviceType">The service with one bridge handler method.</param>
        /// <param name="methodName">The name of bridge handler method.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddBridgeHandlerMethodDefined(
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
                    "Error when creating a pipeline bridge handler resolver."
                    + $" Can't find the bridge handler method with name '{methodName}' for service '{serviceType.FullName}'.");

            if (methods.Count > 1)
                throw new InvalidOperationException(
                    "Error when creating a pipeline bridge handler resolver."
                    + $" There are more then one method with name '{methodName}' for service '{serviceType.FullName}'.");

            return builder.BridgeHandle(methods[0]);
        }

        /// <summary>
        /// Add a handler method to <paramref name="builder"/> from a service finding the method by name.
        /// </summary>
        /// <typeparam name="TService">The service with one handler method.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="methodName">The name of handler method.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddBridgeHandlerMethodDefined<TService>(
            this IPipelineBuilder<TService> builder,
            string methodName)
        {
            return builder.AddBridgeHandlerMethodDefined(typeof(TService), methodName);
        }

        #endregion

        #region Decorators Handlers TIn

        public static IPipelineBuilder<TInput> Decorate<TInput>(this IPipelineBuilder<TInput> builder, Action<TInput, Action> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder;
        }

        public static IPipelineBuilder<TInput> Decorate<TInput>(this IPipelineBuilder<TInput> builder, Func<TInput, Func<Task>, Task> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder;
        }

        public static IPipelineBuilder<TInput> Decorate<TInput>(this IPipelineBuilder<TInput> builder, Func<TInput, Func<Task>, CancellationToken, Task> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder;
        }



        public static IPipelineBuilder<TInput> Decorate<TService, TInput>(this IPipelineBuilderWithService<TService, TInput> builder, Action<TService, TInput, Action> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder.Configure<TInput>();
        }

        public static IPipelineBuilder<TInput> Decorate<TService, TInput>(this IPipelineBuilderWithService<TService, TInput> builder, Func<TService, TInput, Func<Task>, Task> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder.Configure<TInput>();
        }

        public static IPipelineBuilder<TInput> Decorate<TService, TInput>(this IPipelineBuilderWithService<TService, TInput> builder, Func<TService, TInput, Func<Task>, CancellationToken, Task> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder.Configure<TInput>();
        }

        #endregion

        #region Decorators Handlers TIn TOut

        public static IPipelineBuilder<TInput, TOutput> Decorate<TInput, TOutput>(this IPipelineBuilder<TInput, TOutput> builder, Func<TInput, Func<TOutput>, TOutput> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder;
        }

        public static IPipelineBuilder<TInput, TOutput> Decorate<TInput, TOutput>(this IPipelineBuilder<TInput, TOutput> builder, Func<TInput, Func<Task<TOutput>>, Task<TOutput>> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder;
        }

        public static IPipelineBuilder<TInput, TOutput> Decorate<TInput, TOutput>(this IPipelineBuilder<TInput, TOutput> builder, Func<TInput, Func<Task<TOutput>>, CancellationToken, Task<TOutput>> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder;
        }



        public static IPipelineBuilder<TInput, TOutput> Decorate<TService, TInput, TOutput>(this IPipelineBuilderWithService<TService, TInput, TOutput> builder, Func<TService, TInput, Func<TOutput>, TOutput> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder.Configure<TInput, TOutput>();
        }

        public static IPipelineBuilder<TInput, TOutput> Decorate<TService, TInput, TOutput>(this IPipelineBuilderWithService<TService, TInput, TOutput> builder, Func<TService, TInput, Func<Task<TOutput>>, Task<TOutput>> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder.Configure<TInput, TOutput>();
        }

        public static IPipelineBuilder<TInput, TOutput> Decorate<TService, TInput, TOutput>(this IPipelineBuilderWithService<TService, TInput, TOutput> builder, Func<TService, TInput, Func<Task<TOutput>>, CancellationToken, Task<TOutput>> handler)
        {
            builder.AddDecoratorResolver(DefaultDecoratorsResolver.Decorate(handler));
            return builder.Configure<TInput, TOutput>();
        }

        #endregion

        #region Method Decorators Handlers

        /// <summary>
        /// Add a method as a decorator handler into pipeline builder.
        /// </summary>
        /// <param name="builder">The pipeline builder for configure.</param>
        /// <param name="method">The decorator handler method.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder Decorate(this IPipelineBuilder builder, MethodInfo method)
        {
            builder.AddDecoratorResolver(new MethodDecoratorResolver(method));
            return builder;
        }

        /// <summary>
        /// Adds many methods as decorator handlers into the pipeline builder.
        /// </summary>
        /// <param name="builder">The pipeline builder for configure.</param>
        /// <param name="methods">The decorator handlers methods</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddDecoratorsMethods(this IPipelineBuilder builder, params MethodInfo[] methods)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (methods is null)
                throw new ArgumentNullException(nameof(methods));

            foreach (var method in methods)
            {
                builder.AddDecoratorResolver(new MethodDecoratorResolver(method));
            }

            return builder;
        }

        /// <summary>
        /// Adds all methods from a service to <paramref name="builder"/> as decorators handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="serviceType">The service with decorators handlers methods.</param>
        /// <param name="attributeType">The attribute that define a method as decorator handler.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddDecoratorsMethodsDefined(
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

            return builder.AddDecoratorsMethods(methods);
        }

        /// <summary>
        /// Adds all methods from a service to <paramref name="builder"/> as decorators handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <typeparam name="TService">The service with decorators handlers methods.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="attributeType">The attribute that define a method as decorator handler.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddDecoratorsMethodsDefined<TService>(
            this IPipelineBuilderWithService<TService> builder,
            Type attributeType)
        {
            return builder.AddDecoratorsMethodsDefined(typeof(TService), attributeType);
        }

        /// <summary>
        /// Adds all methods from a service to <paramref name="builder"/> as decorators handlers
        /// where the methods are defined with some attribute.
        /// </summary>
        /// <typeparam name="TService">The service with decorators handlers methods.</typeparam>
        /// <typeparam name="TAttribute">The attribute that define a method as decorator handler.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddDecoratorsMethodsDefined<TService, TAttribute>(
            this IPipelineBuilderWithService<TService> builder)
            where TAttribute : Attribute
        {
            return builder.AddDecoratorsMethodsDefined(typeof(TService), typeof(TAttribute));
        }

        /// <summary>
        /// Add a decorator handler method to <paramref name="builder"/> from a service finding the method by name.
        /// </summary>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="serviceType">The service with one decorator handler method.</param>
        /// <param name="methodName">The name of decorator handler method.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddDecoratorMethodDefined(
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
                    "Error when creating a pipeline decorator handler resolver."
                    + $" Can't find the decorator handler method with name '{methodName}' for service '{serviceType.FullName}'.");

            if (methods.Count > 1)
                throw new InvalidOperationException(
                    "Error when creating a pipeline decorator handler resolver."
                    + $" There are more then one method with name '{methodName}' for service '{serviceType.FullName}'.");

            return builder.Decorate(methods[0]);
        }

        /// <summary>
        /// Add a decorator handler method to <paramref name="builder"/> from a service finding the method by name.
        /// </summary>
        /// <typeparam name="TService">The service with one decorator handler method.</typeparam>
        /// <param name="builder">The <see cref="IPipelineBuilder"/> to configure.</param>
        /// <param name="methodName">The name of decorator handler method.</param>
        /// <returns>The same instance of <paramref name="builder"/> for chain calls.</returns>
        public static IPipelineBuilder AddDecoratorMethodDefined<TService>(
            this IPipelineBuilder<TService> builder,
            string methodName)
        {
            return builder.AddDecoratorMethodDefined(typeof(TService), methodName);
        }

        #endregion
    }
}
