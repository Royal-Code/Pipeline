using Microsoft.Extensions.DependencyInjection.Extensions;
using RoyalCode.CommandAndQuery;
using RoyalCode.PipelineFlow;
using RoyalCode.PipelineFlow.Builders;
using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.CommandAndQuery.Internal;
using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.PipelineFlow.Extensions;
using RoyalCode.PipelineFlow.Resolvers;
using System;
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

            configuration.Configuration.Handlers.Add(CommandQueryHandlerResolver.ForHandlerRequest());
            configuration.Configuration.Handlers.Add(CommandQueryHandlerResolver.ForHandlerRequestResult());
            configuration.Configuration.Handlers.Add(CommandQueryHandlerResolver.ForAsyncHandlerRequest());
            configuration.Configuration.Handlers.Add(CommandQueryHandlerResolver.ForAsyncHandlerRequestResult());

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
            var configuration = services
                .Where(s => s.ServiceType == typeof(PipelineFactoryConfiguration<ICommandQueryBus>))
                .Select(s => (PipelineFactoryConfiguration<ICommandQueryBus>)s.ImplementationInstance)
                .FirstOrDefault();

            if (configuration is null)
            {
                configuration = PipelineFactory.Configure<ICommandQueryBus>();
                services.AddSingleton(configuration);

                services.AddTransient<ICommandQueryBus, CommandQueryBus>();
                services.AddSingleton<RequestDispatchers>();
                services.AddSingleton(sp =>
                {
                    var configuration = sp.GetRequiredService<PipelineFactoryConfiguration<ICommandQueryBus>>();
                    return configuration.CreatePipelineChainTypeBuilder();
                });
                services.AddTransient<IPipelineTypeBuilder, PipelineTypeBuilder>();
                services.AddTransient<IPipelineFactory<ICommandQueryBus>, PipelineFactory<ICommandQueryBus>>();

                services.AddPipelineChains();
            }

            return configuration;
        }

        /// <summary>
        /// Adds the Chains used by the Pipeline as services.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns>The same instance of <paramref name="services"/> to chain calls.</returns>
        public static IServiceCollection AddPipelineChains(this IServiceCollection services)
        {
            if (services.Any(d => d.ImplementationInstance is ChainDelegateRegistry))
                return services;

            services.AddSingleton(PipelineFactory.ChainDelegateRegistry);
            services.AddTransient(typeof(IChainDelegateProvider<>), typeof(ChainDelegateProvider<>));

            services.AddTransient(typeof(HandlerChainDelegateAsync<>));
            services.AddTransient(typeof(HandlerChainDelegateAsync<,>));
            services.AddTransient(typeof(HandlerChainDelegateSync<>));
            services.AddTransient(typeof(HandlerChainDelegateSync<,>));
            services.AddTransient(typeof(HandlerChainDelegateWithoutCancellationTokenAsync<>));
            services.AddTransient(typeof(HandlerChainDelegateWithoutCancellationTokenAsync<,>));

            services.AddTransient(typeof(HandlerChainServiceAsync<,>));
            services.AddTransient(typeof(HandlerChainServiceAsync<,,>));
            services.AddTransient(typeof(HandlerChainServiceSync<,>));
            services.AddTransient(typeof(HandlerChainServiceSync<,,>));
            services.AddTransient(typeof(HandlerChainServiceWithoutCancellationTokenAsync<,>));
            services.AddTransient(typeof(HandlerChainServiceWithoutCancellationTokenAsync<,,>));

            services.AddTransient(typeof(DecoratorChainDelegateAsync<,>));
            services.AddTransient(typeof(DecoratorChainDelegateAsync<,,>));
            services.AddTransient(typeof(DecoratorChainDelegateSync<,>));
            services.AddTransient(typeof(DecoratorChainDelegateSync<,,>));
            services.AddTransient(typeof(DecoratorChainDelegateWithoutCancellationTokenAsync<,>));
            services.AddTransient(typeof(DecoratorChainDelegateWithoutCancellationTokenAsync<,,>));

            services.AddTransient(typeof(DecoratorChainServiceAsync<,,>));
            services.AddTransient(typeof(DecoratorChainServiceAsync<,,,>));
            services.AddTransient(typeof(DecoratorChainServiceSync<,,>));
            services.AddTransient(typeof(DecoratorChainServiceSync<,,,>));
            services.AddTransient(typeof(DecoratorChainServiceWithoutCancellationTokenAsync<,,>));
            services.AddTransient(typeof(DecoratorChainServiceWithoutCancellationTokenAsync<,,,>));

            services.AddTransient(typeof(BridgeChainDelegateAsync<,,>));
            services.AddTransient(typeof(BridgeChainDelegateAsync<,,,>));
            services.AddTransient(typeof(BridgeChainDelegateAsync<,,,,>));
            services.AddTransient(typeof(BridgeChainDelegateSync<,,>));
            services.AddTransient(typeof(BridgeChainDelegateSync<,,,>));
            services.AddTransient(typeof(BridgeChainDelegateSync<,,,,>));
            services.AddTransient(typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,>));
            services.AddTransient(typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,,>));
            services.AddTransient(typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,,,>));

            services.AddTransient(typeof(BridgeChainServiceAsync<,,,>));
            services.AddTransient(typeof(BridgeChainServiceAsync<,,,,>));
            services.AddTransient(typeof(BridgeChainServiceAsync<,,,,,>));
            services.AddTransient(typeof(BridgeChainServiceSync<,,,>));
            services.AddTransient(typeof(BridgeChainServiceSync<,,,,>));
            services.AddTransient(typeof(BridgeChainServiceSync<,,,,,>));
            services.AddTransient(typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,>));
            services.AddTransient(typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,,>));
            services.AddTransient(typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,,,>));

            return services;
        }

        private class CommandQueryHandlerResolver : MethodHandlerResolver
        {
            private CommandQueryHandlerResolver(MethodInfo method) 
                : base(method)
            {
                IsFallback = true;
            }

            public static CommandQueryHandlerResolver ForHandlerRequest()
            {
                var method = typeof(IHandler<>).GetMethod(nameof(IHandler<IRequest>.Handle));
                return new CommandQueryHandlerResolver(method);
            }

            public static CommandQueryHandlerResolver ForHandlerRequestResult()
            {
                var method = typeof(IHandler<,>).GetMethod(nameof(IHandler<IRequest>.Handle));
                return new CommandQueryHandlerResolver(method);
            }

            public static CommandQueryHandlerResolver ForAsyncHandlerRequest()
            {
                var method = typeof(IAsyncHandler<>).GetMethod(nameof(IAsyncHandler<IRequest>.HandleAsync));
                return new CommandQueryHandlerResolver(method);
            }

            public static CommandQueryHandlerResolver ForAsyncHandlerRequestResult()
            {
                var method = typeof(IAsyncHandler<,>).GetMethod(nameof(IAsyncHandler<IRequest>.HandleAsync));
                return new CommandQueryHandlerResolver(method);
            }
        }
    }
}
