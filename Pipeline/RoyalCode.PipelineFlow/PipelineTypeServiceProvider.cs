using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace RoyalCode.PipelineFlow
{
    /// <summary>
    /// When not using the AspNetCore Dependency Injection, or other Container implementation,
    /// this component supply the needs for create instances of pipelines, chains and services.
    /// </summary>
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

        public object? GetService(Type requiredServiceType)
        {
            if (GuardLoop(requiredServiceType))
            {
                throw new InvalidOperationException($"A a dependency loop was found.\n{GetDependencyTree()}");
            }

            var serviceType = serviceFactoryCollection.CheckServiceMap(requiredServiceType);

            var factory = serviceFactoryCollection.GetFactory(serviceType);
            if (factory is null)
            {
                if (ServiceActivator.CanActivate(serviceType, serviceFactoryCollection))
                    return GetService(serviceType);

                throw new ArgumentException(
                    $"The type '{serviceType.FullName}' is not a service and can't be created from the default construtor", nameof(serviceType));
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

    /// <summary>
    /// The activator search for the type dependencies and create a lambda expression to create new instances for some
    /// type.
    /// </summary>
    internal static class ServiceActivator
    {
        internal static bool CanActivate(Type type, ServiceFactoryCollection serviceFactories)
        {
            var ctor = type.GetConstructors()
                .Where(c => c.IsPublic)
                .FirstOrDefault();

            if (ctor is null)
                return false;

            var dependencies = ctor.GetParameters().Select(p => new Dependency(p)).ToList();

            foreach (var dependency in dependencies)
            {
                var serviceType = dependency.ParameterInfo.ParameterType;
                if (serviceFactories.IsRegistered(serviceType))
                    continue;

                if (!CanActivate(serviceType, serviceFactories))
                {
                    if (dependency.ParameterInfo.IsOptional)
                        dependency.UseNullValue = true;
                    else
                        return false;
                }
            }

            CreateFactory(type, serviceFactories, ctor, dependencies);

            return true;
        }

        private static void CreateFactory(
            Type type,
            ServiceFactoryCollection serviceFactories,
            ConstructorInfo ctor,
            List<Dependency> dependencies)
        {
            var typeParam = Expression.Parameter(typeof(Type), "type");
            var spParam = Expression.Parameter(typeof(IServiceProvider), "sp");

            var newExpression = Expression.New(ctor, dependencies.Select(d =>
            {
                return (Expression)(d.UseNullValue
                    ? Expression.Constant(null, d.ParameterInfo.ParameterType)
                    : Expression.Convert(
                        Expression.Call(
                            spParam,
                            typeof(IServiceProvider).GetMethod("GetService")!,
                            Expression.Constant(d.ParameterInfo.ParameterType)),
                        d.ParameterInfo.ParameterType));
            }));

            var lambda = Expression.Lambda<Func<Type, IServiceProvider, object>>(newExpression, typeParam, spParam);

            var factory = lambda.Compile();

            serviceFactories.AddServiceFactory(type, factory);
        }

        private class Dependency
        {
            public Dependency(ParameterInfo parameterInfo)
            {
                ParameterInfo = parameterInfo;
            }

            public ParameterInfo ParameterInfo { get; }

            public bool UseNullValue { get; set; } = false;
        }
    }
}
