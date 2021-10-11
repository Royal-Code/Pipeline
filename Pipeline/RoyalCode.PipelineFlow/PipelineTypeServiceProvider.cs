using System;
using System.Text;

namespace RoyalCode.PipelineFlow
{
    internal class PipelineTypeServiceProvider : IServiceProvider
    {
        private readonly ServiceFactoryCollection serviceFactoryCollection;
        private readonly PipelineTypeServiceProvider? previous;
        private readonly Type? requestedServiceType;

        internal PipelineTypeServiceProvider(ServiceFactoryCollection serviceFactoryCollection)
        {
            this.serviceFactoryCollection = serviceFactoryCollection;
        }

        internal PipelineTypeServiceProvider(
            ServiceFactoryCollection serviceFactoryCollection, 
            PipelineTypeServiceProvider? previous, 
            Type? requestedServiceType) : this(serviceFactoryCollection)
        {
            this.previous = previous;
            this.requestedServiceType = requestedServiceType;
        }

        internal bool GuardLoop(Type serviceType)
        {
            if (requestedServiceType is not null
                && (serviceType == requestedServiceType || previous!.GuardLoop(serviceType)))
            {
                return true;
            }

            return false;
        }

        internal StringBuilder? GetDependencyTree()
        {
            if (previous is null)
                return null;

            var sb = previous.GetDependencyTree();
            if (sb is null)
                sb = new StringBuilder($"The requested service was {requestedServiceType!.FullName}\n");
            else
                sb.AppendLine($"It depends on the {requestedServiceType!.FullName} service.");

            return sb;
        }

        public object? GetService(Type serviceType)
        {
            if (GuardLoop(serviceType))
            {
                throw new InvalidOperationException($"A a dependency loop was found.\n{GetDependencyTree()}");
            }

            var factory = serviceFactoryCollection.GetFactory(serviceType);
            if (factory is null)
            {
                try
                {
                    return Activator.CreateInstance(serviceType);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(
                        $"The type '{serviceType.FullName}' is not a service and can't be created from the default construtor", nameof(serviceType), ex);
                }
            }

            var provider = new PipelineTypeServiceProvider(serviceFactoryCollection, this, serviceType);
            var instance = factory(serviceType, provider);

            if (instance is not null && !serviceType.IsAssignableFrom(instance.GetType()))
            {
                throw new InvalidOperationException("The service factory returns a invalid instance. " +
                    $"The requested service type is '{serviceType.FullName}', " +
                    $"and the returned service is '{instance.GetType().FullName}'.");
            }

            return instance;
        }
    }
}
