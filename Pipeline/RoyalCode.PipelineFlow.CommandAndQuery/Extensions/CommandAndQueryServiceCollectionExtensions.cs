using RoyalCode.CommandAndQuery;
using RoyalCode.PipelineFlow;
using RoyalCode.PipelineFlow.Builders;
using RoyalCode.PipelineFlow.CommandAndQuery.Internal;
using RoyalCode.PipelineFlow.Configurations;
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
        /// <param name="services">IServiceCollection.</param>
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

        public static IServiceCollection AddCommandsAndQueriesFromAssembly(Assembly assembly)
        {

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

                services.AddPipelineCore();
            }

            return configuration;
        }

        private static void AddPipelineCore(this IServiceCollection services)
        {
            // requer todos chains, e outras classes se necessário.
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
