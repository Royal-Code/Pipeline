using Microsoft.Extensions.DependencyInjection.Extensions;
using RoyalCode.CommandAndQuery;
using RoyalCode.PipelineFlow;
using RoyalCode.PipelineFlow.CommandAndQuery.Internal;
using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.PipelineFlow.Descriptors;
using RoyalCode.PipelineFlow.Extensions;
using RoyalCode.PipelineFlow.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class CommandAndQueryServiceCollectionExtensions
    {
        /// <summary>
        /// Add the services for <see cref="ICommandQueryBus"/> and, optionally, configure the pipeline.
        /// </summary>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="pipelineConfigureAction">Action to configure the pipeline, optional.</param>
        /// <returns>The same instance of <paramref name="services"/> to chain calls.</returns>
        public static IServiceCollection AddCommandQueryBus(this IServiceCollection services,
            Action<IPipelineBuilder>? pipelineConfigureAction = null)
        {
            var configuration = services.GetPipelineFactoryConfiguration();

            if (pipelineConfigureAction is not null)
            {
                configuration.ConfigurePipelines(pipelineConfigureAction);
            }

            return services;
        }

        /// <summary>
        /// Add the command and query handlers as a service. 
        /// This addition will make fallback handlers for any command or query that will use handler services.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns>The same instance of <paramref name="services"/> to chain calls.</returns>
        public static IServiceCollection AddCommandAndQueryHandlersAsAService(this IServiceCollection services)
        {
            var configuration = services.GetPipelineFactoryConfiguration();

            var shr = ServiceHandlersRegistry.GetOrCreate(services);

            configuration.Configuration.Handlers.Add(CommandQueryHandlerResolver.ForHandlerRequest(shr));
            configuration.Configuration.Handlers.Add(CommandQueryHandlerResolver.ForHandlerRequestResult(shr));
            configuration.Configuration.Handlers.Add(CommandQueryHandlerResolver.ForAsyncHandlerRequest(shr));
            configuration.Configuration.Handlers.Add(CommandQueryHandlerResolver.ForAsyncHandlerRequestResult(shr));

            return services;
        }

        /// <summary>
        /// <para>
        ///     From the classes in an assembly of the type <typeparamref name="T"/>, 
        ///     check which classes implement some handler, decorator or bridge 
        ///     and register them as command and query handlers.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The type for get the assembly.</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns>The same instance of <paramref name="services"/> to chain calls.</returns>
        public static IServiceCollection AddCommandsAndQueriesFromAssemblyOfType<T>(this IServiceCollection services)
            => services.AddCommandsAndQueriesFromAssembly(typeof(T).Assembly);

        /// <summary>
        /// <para>
        ///     From the classes in an assembly, check which classes implement some handler, decorator or bridge 
        ///     and register them as command and query handlers.
        /// </para>
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="assembly">An Assembly with the types to be registered.</param>
        /// <returns>The same instance of <paramref name="services"/> to chain calls.</returns>
        public static IServiceCollection AddCommandsAndQueriesFromAssembly(
            this IServiceCollection services,
            Assembly assembly)
        {
            var configuration = services.GetPipelineFactoryConfiguration();

            configuration.ConfigurePipelines(builder =>
            {
                foreach (var type in assembly.GetTypes())
                {
                    bool addTypeAsService = false;
                    if (type.Implements(typeof(IHandler<>)) || type.Implements(typeof(IHandler<,>)))
                    {
                        builder.AddHandlerMethodDefined(type, nameof(IHandler<IRequest>.Handle));
                        addTypeAsService = true;
                    }
                    else if (type.Implements(typeof(IDecorator<>)) || type.Implements(typeof(IDecorator<,>)))
                    {
                        builder.AddDecoratorMethodDefined(type, nameof(IDecorator<IRequest>.Handle));
                        addTypeAsService = true;
                    }
                    else if (type.Implements(typeof(IBridge<,>))
                        || type.Implements(typeof(IBridge<,,>)) || type.Implements(typeof(IBridge<,,,>)))
                    {
                        builder.AddBridgeHandlerMethodDefined(type, nameof(IBridge<IRequest, IRequest>.Next));
                        addTypeAsService = true;
                    }
                    else if (type.Implements(typeof(IAsyncHandler<>)) || type.Implements(typeof(IAsyncHandler<,>)))
                    {
                        builder.AddHandlerMethodDefined(type, nameof(IAsyncHandler<IRequest>.HandleAsync));
                        addTypeAsService = true;
                    }
                    else if (type.Implements(typeof(IAsyncDecorator<>)) || type.Implements(typeof(IAsyncDecorator<,>)))
                    {
                        builder.AddDecoratorMethodDefined(type, nameof(IAsyncDecorator<IRequest>.HandleAsync));
                        addTypeAsService = true;
                    }
                    else if (type.Implements(typeof(IAsyncBridge<,>))
                        || type.Implements(typeof(IAsyncBridge<,,>)) || type.Implements(typeof(IAsyncBridge<,,,>)))
                    {
                        builder.AddBridgeHandlerMethodDefined(type, nameof(IAsyncBridge<IRequest, IRequest>.NextAsync));
                        addTypeAsService = true;
                    }
                    if (addTypeAsService)
                        services.TryAddTransient(type);
                }
            });

            return services;
        }

        private static PipelineFactoryConfiguration<ICommandQueryBus> GetPipelineFactoryConfiguration(
            this IServiceCollection services)
        {
            return services.GetPipelineFactoryConfiguration<ICommandQueryBus>(static (cfg, sc) =>
            {
                sc.AddTransient<ICommandQueryBus, CommandQueryBus>();
            });
        }

        private class CommandQueryHandlerResolver : MethodHandlerResolver
        {
            private readonly ServiceHandlersRegistry serviceHandlersRegistry;
            private readonly bool isAsync;
            private CommandQueryHandlerResolver(
                MethodInfo method,
                ServiceHandlersRegistry serviceHandlersRegistry, 
                bool isAsync) : base(method)
            {
                IsFallback = true;
                this.serviceHandlersRegistry = serviceHandlersRegistry;
                this.isAsync = isAsync;
            }

            public override HandlerDescriptor? TryResolve(Type inputType)
            {
                if (isAsync)
                {
                    if (!serviceHandlersRegistry.HasAsyncHandlerServiceFor(inputType))
                        return null;
                }
                else
                {
                    if (!serviceHandlersRegistry.HasHandlerServiceFor(inputType))
                        return null;
                }

                return base.TryResolve(inputType);
            }

            public override HandlerDescriptor? TryResolve(Type inputType, Type output)
            {
                if (isAsync)
                {
                    if (!serviceHandlersRegistry.HasAsyncHandlerServiceFor(inputType, output))
                        return null;
                }
                else
                {
                    if (!serviceHandlersRegistry.HasHandlerServiceFor(inputType, output))
                        return null;
                }

                return base.TryResolve(inputType, output);
            }

            public static CommandQueryHandlerResolver ForHandlerRequest(ServiceHandlersRegistry serviceHandlersRegistry)
            {
                var method = typeof(IHandler<>).GetMethod(nameof(IHandler<IRequest>.Handle)) 
                    ?? throw new ArgumentException("Invalid handler method");
                return new CommandQueryHandlerResolver(method, serviceHandlersRegistry, false);
            }

            public static CommandQueryHandlerResolver ForHandlerRequestResult(ServiceHandlersRegistry serviceHandlersRegistry)
            {
                var method = typeof(IHandler<,>).GetMethod(nameof(IHandler<IRequest>.Handle))
                    ?? throw new ArgumentException("Invalid handler method");
                return new CommandQueryHandlerResolver(method, serviceHandlersRegistry, false);
            }

            public static CommandQueryHandlerResolver ForAsyncHandlerRequest(ServiceHandlersRegistry serviceHandlersRegistry)
            {
                var method = typeof(IAsyncHandler<>).GetMethod(nameof(IAsyncHandler<IRequest>.HandleAsync))
                    ?? throw new ArgumentException("Invalid handler method");
                return new CommandQueryHandlerResolver(method, serviceHandlersRegistry, true);
            }

            public static CommandQueryHandlerResolver ForAsyncHandlerRequestResult(ServiceHandlersRegistry serviceHandlersRegistry)
            {
                var method = typeof(IAsyncHandler<,>).GetMethod(nameof(IAsyncHandler<IRequest>.HandleAsync))
                    ?? throw new ArgumentException("Invalid handler method");
                return new CommandQueryHandlerResolver(method, serviceHandlersRegistry, true);
            }
        }

        private class ServiceHandlersRegistry
        {
            private IServiceCollection? services;
            private IEnumerable<Type>? handlersServicesTypes;

            private ServiceHandlersRegistry(IServiceCollection services)
            {
                this.services = services;
            }

            public static ServiceHandlersRegistry GetOrCreate(IServiceCollection services)
            {
                var service = services.FirstOrDefault(d => d.ServiceType == typeof(ServiceHandlersRegistry))?.ImplementationInstance;

                if(service is not null)
                    return (ServiceHandlersRegistry)service!;

                var instance = new ServiceHandlersRegistry(services);
                services.AddSingleton(instance);
                return instance;
            }

            private void Initialize()
            {
                if (services == null)
                    return;

                handlersServicesTypes = services.Where(d => 
                        d.ServiceType.Implements(typeof(IHandler<>))
                        || d.ServiceType.Implements(typeof(IAsyncHandler<>))
                        || d.ServiceType.Implements(typeof(IHandler<,>))
                        || d.ServiceType.Implements(typeof(IAsyncHandler<,>)))
                    .Select(d => d.ServiceType)
                    .ToList();

                services = null;
            }

            public bool HasHandlerServiceFor(Type inputType)
            {
                Initialize();
                var handlerType = typeof(IHandler<>).MakeGenericType(inputType);
                return handlersServicesTypes!.Any(Match(handlerType));
            }

            public bool HasAsyncHandlerServiceFor(Type inputType)
            {
                Initialize();
                var handlerType = typeof(IAsyncHandler<>).MakeGenericType(inputType);
                return handlersServicesTypes!.Any(Match(handlerType));
            }

            public bool HasHandlerServiceFor(Type inputType, Type outputType)
            {
                Initialize();
                var handlerType = typeof(IHandler<,>).MakeGenericType(inputType, outputType);
                return handlersServicesTypes!.Any(Match(handlerType));
            }

            public bool HasAsyncHandlerServiceFor(Type inputType, Type outputType)
            {
                Initialize();
                var handlerType = typeof(IAsyncHandler<,>).MakeGenericType(inputType, outputType);
                return handlersServicesTypes!.Any(Match(handlerType));
            }

            private Func<Type, bool> Match(Type handlerType)
                => t => t == handlerType || (t.IsGenericTypeDefinition && t == handlerType.GetGenericTypeDefinition());
        }
    }
}
