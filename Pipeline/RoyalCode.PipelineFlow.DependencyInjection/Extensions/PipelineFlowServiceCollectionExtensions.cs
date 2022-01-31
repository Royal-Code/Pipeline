using Microsoft.Extensions.DependencyInjection.Extensions;
using RoyalCode.PipelineFlow;
using RoyalCode.PipelineFlow.Builders;
using RoyalCode.PipelineFlow.Chains;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// <para>
///     Extension methods for <see cref="IServiceCollection"/>.
/// </para>
/// </summary>
public static class PipelineFlowServiceCollectionExtensions
{
    /// <summary>
    /// <para>
    ///     Get or create a <see cref="PipelineFactoryConfiguration{TFor}"/> 
    ///     for a given component <typeparamref name="TFor"/>.
    /// </para>
    /// <para>
    ///     This method is not design to be used direct by the application, but for used by other
    ///     extension methods related to the <typeparamref name="TFor"/> component.
    /// </para>
    /// <para>
    ///     That will be one instance of <see cref="PipelineFactoryConfiguration{TFor}"/> for each
    ///     <typeparamref name="TFor"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TFor">The component type that will use the pipeline.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="whenCreatedAction">
    /// <para>
    ///     Optional action to configure services or the configuration when the 
    ///     <see cref="PipelineFactoryConfiguration{TFor}"/> is created.
    /// </para>
    /// </param>
    /// <returns>The pipeline configuration.</returns>
    public static PipelineFactoryConfiguration<TFor> GetPipelineFactoryConfiguration<TFor>(
        this IServiceCollection services,
        Action<PipelineFactoryConfiguration<TFor>, IServiceCollection>? whenCreatedAction = null)
    {
        var configuration = services
            .Where(s => s.ServiceType == typeof(PipelineFactoryConfiguration<TFor>))
            .Select(s => s.ImplementationInstance)
            .OfType<PipelineFactoryConfiguration<TFor>>()
            .FirstOrDefault();

        if (configuration is null)
        {
            configuration = PipelineFactory.Configure<TFor>();
            services.AddSingleton(configuration);

            whenCreatedAction?.Invoke(configuration, services);

            services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<PipelineFactoryConfiguration<TFor>>();
                return configuration.CreatePipelineChainTypeBuilder();
            });
            services.TryAddTransient<IPipelineTypeBuilder, PipelineTypeBuilder>();
            services.TryAddTransient<IPipelineFactory<TFor>, PipelineFactory<TFor>>();

            services.AddPipelineChains();
        }

        return configuration;
    }

    /// <summary>
    /// <para>
    ///     Adds the Chains used by the Pipeline as services.
    /// </para>
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <returns>The same instance of <paramref name="services"/> to chain calls.</returns>
    private static IServiceCollection AddPipelineChains(this IServiceCollection services)
    {
        if (services.Any(d => d.ImplementationInstance is ChainDelegateRegistry))
            return services;

        services.TryAddSingleton(PipelineFactory.ChainDelegateRegistry);
        services.TryAddSingleton(typeof(IChainDelegateProvider<>), typeof(ChainDelegateProvider<>));
        services.TryAddTransient(typeof(PipelineCaller<,>));
        services.TryAddTransient(typeof(PipelineCaller<,,>));

        services.TryAddTransient(typeof(HandlerChainDelegateAsync<>));
        services.TryAddTransient(typeof(HandlerChainDelegateAsync<,>));
        services.TryAddTransient(typeof(HandlerChainDelegateSync<>));
        services.TryAddTransient(typeof(HandlerChainDelegateSync<,>));
        services.TryAddTransient(typeof(HandlerChainDelegateWithoutCancellationTokenAsync<>));
        services.TryAddTransient(typeof(HandlerChainDelegateWithoutCancellationTokenAsync<,>));

        services.TryAddTransient(typeof(HandlerChainServiceAsync<,>));
        services.TryAddTransient(typeof(HandlerChainServiceAsync<,,>));
        services.TryAddTransient(typeof(HandlerChainServiceSync<,>));
        services.TryAddTransient(typeof(HandlerChainServiceSync<,,>));
        services.TryAddTransient(typeof(HandlerChainServiceWithoutCancellationTokenAsync<,>));
        services.TryAddTransient(typeof(HandlerChainServiceWithoutCancellationTokenAsync<,,>));

        services.TryAddTransient(typeof(DecoratorChainDelegateAsync<,>));
        services.TryAddTransient(typeof(DecoratorChainDelegateAsync<,,>));
        services.TryAddTransient(typeof(DecoratorChainDelegateSync<,>));
        services.TryAddTransient(typeof(DecoratorChainDelegateSync<,,>));
        services.TryAddTransient(typeof(DecoratorChainDelegateWithoutCancellationTokenAsync<,>));
        services.TryAddTransient(typeof(DecoratorChainDelegateWithoutCancellationTokenAsync<,,>));

        services.TryAddTransient(typeof(DecoratorChainServiceAsync<,,>));
        services.TryAddTransient(typeof(DecoratorChainServiceAsync<,,,>));
        services.TryAddTransient(typeof(DecoratorChainServiceSync<,,>));
        services.TryAddTransient(typeof(DecoratorChainServiceSync<,,,>));
        services.TryAddTransient(typeof(DecoratorChainServiceWithoutCancellationTokenAsync<,,>));
        services.TryAddTransient(typeof(DecoratorChainServiceWithoutCancellationTokenAsync<,,,>));

        services.TryAddTransient(typeof(BridgeChainDelegateAsync<,,>));
        services.TryAddTransient(typeof(BridgeChainDelegateAsync<,,,>));
        services.TryAddTransient(typeof(BridgeChainDelegateAsync<,,,,>));
        services.TryAddTransient(typeof(BridgeChainDelegateSync<,,>));
        services.TryAddTransient(typeof(BridgeChainDelegateSync<,,,>));
        services.TryAddTransient(typeof(BridgeChainDelegateSync<,,,,>));
        services.TryAddTransient(typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,>));
        services.TryAddTransient(typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,,>));
        services.TryAddTransient(typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,,,>));

        services.TryAddTransient(typeof(BridgeChainServiceAsync<,,,>));
        services.TryAddTransient(typeof(BridgeChainServiceAsync<,,,,>));
        services.TryAddTransient(typeof(BridgeChainServiceAsync<,,,,,>));
        services.TryAddTransient(typeof(BridgeChainServiceSync<,,,>));
        services.TryAddTransient(typeof(BridgeChainServiceSync<,,,,>));
        services.TryAddTransient(typeof(BridgeChainServiceSync<,,,,,>));
        services.TryAddTransient(typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,>));
        services.TryAddTransient(typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,,>));
        services.TryAddTransient(typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,,,>));

        return services;
    }
}

