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
    }
}
