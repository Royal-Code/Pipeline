using RoyalCode.PipelineFlow.Extensions;
using System;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow
{
    /// <summary>
    /// <para>
    ///     Collection of factories for services.
    /// </para>
    /// </summary>
    public class ServiceFactoryCollection
    {
        private readonly Dictionary<Type, Func<Type, IServiceProvider, object>> factories = new();
        private readonly Dictionary<Type, Type> serviceMap = new();

        /// <summary>
        /// Add a service factory.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="factory">The factory delegate.</param>
        /// <returns>The same collection for chain calls.</returns>
        public ServiceFactoryCollection AddServiceFactory<TService>(Func<Type, IServiceProvider, object> factory)
            => AddServiceFactory(typeof(TService), factory);

        /// <summary>
        /// Add a service factory.
        /// </summary>
        /// <param name="serviceType">The service type, can be generic.</param>
        /// <param name="factory">The factory delegate.</param>
        /// <returns>The same collection for chain calls.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para>
        ///     If the <paramref name="factory"/> are null.
        /// </para>
        /// <para>
        ///     If the <paramref name="serviceType"/> are null.
        /// </para>
        /// </exception>
        public ServiceFactoryCollection AddServiceFactory(Type serviceType, Func<Type, IServiceProvider, object> factory)
        {
            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));

            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            factories[serviceType] = factory;

            return this;
        }

        /// <summary>
        /// Map a service type to a implementation type.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <returns>The same collection for chain calls.</returns>
        public ServiceFactoryCollection MapService<TService, TImplementation>()
            where TImplementation: TService
        {
            return MapService(typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Map a service type to a implementation type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <returns>The same collection for chain calls.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para>
        ///     If the <paramref name="serviceType"/> are null.
        /// </para>
        /// <para>
        ///     If the <paramref name="implementationType"/> are null.
        /// </para>
        /// </exception>
        public ServiceFactoryCollection MapService(Type serviceType, Type implementationType)
        {
            if (serviceType is null)
                throw new ArgumentNullException(nameof(serviceType));

            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));

            if (!implementationType.Implements(implementationType))
                throw new ArgumentException("The implementation type is not assignable to service type.", nameof(implementationType));

            serviceMap[serviceType] = implementationType;

            return this;
        }

        /// <summary>
        /// Create a <see cref="IServiceProvider"/> instance for build services using the added factories.
        /// </summary>
        /// <returns>A new instance of <see cref="IServiceProvider"/>.</returns>
        public IServiceProvider BuildServiceProvider() => new PipelineTypeServiceProvider(this);

        internal Func<Type, IServiceProvider, object>? GetFactory(Type serviceType)
        {
            return factories.ContainsKey(serviceType)
                ? factories[serviceType] 
                : serviceType.IsGenericType && factories.ContainsKey(serviceType.GetGenericTypeDefinition())
                    ? factories[serviceType.GetGenericTypeDefinition()]
                    : null;
        }

        internal Type CheckServiceMap(Type requiredServiceType)
        {
            return serviceMap.ContainsKey(requiredServiceType)
                ? serviceMap[requiredServiceType]
                : requiredServiceType.IsGenericType && serviceMap.ContainsKey(requiredServiceType.GetGenericTypeDefinition())
                    ? serviceMap[requiredServiceType.GetGenericTypeDefinition()].MakeGenericType(requiredServiceType.GetGenericArguments())
                    : requiredServiceType;
        }

        internal bool IsRegistered(Type serviceType)
        {
            return factories.ContainsKey(serviceType)
                || (serviceType.IsGenericType && factories.ContainsKey(serviceType.GetGenericTypeDefinition()))
                || serviceMap.ContainsKey(serviceType)
                || (serviceType.IsGenericType && serviceMap.ContainsKey(serviceType.GetGenericTypeDefinition()));
        }
    }
}
