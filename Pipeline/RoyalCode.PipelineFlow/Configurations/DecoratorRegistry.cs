using RoyalCode.PipelineFlow.Descriptors;
using RoyalCode.PipelineFlow.Resolvers;
using System;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// <para>
    ///     This class work like a collection to store <see cref="IDecoratorResolver"/> and retrieve <see cref="DecoratorDescriptor"/>.
    /// </para>
    /// <para>
    ///     The <see cref="IPipelineConfiguration"/> holds the <see cref="HandlerRegistry"/> and the <see cref="DecoratorRegistry"/>
    ///     that contains all the handlers and decorators for building pipelines for a "bus" component kind.
    /// </para>
    /// <para>
    ///     Each kind of "bus" component may have its own policies and restrictions on how to build pipelines.
    /// </para>
    /// </summary>
    public class DecoratorRegistry
    {
        private readonly ICollection<IDecoratorResolver> resolvers = new List<IDecoratorResolver>();

        /// <summary>
        /// Adds a decorator descriptor resolver.
        /// </summary>
        /// <param name="decoratorResolver">A resolver for decorator descriptors.</param>
        public void Add(IDecoratorResolver decoratorResolver)
        {
            resolvers.Add(decoratorResolver);
        }

        /// <summary>
        /// Gets all descriptors for a given input type.
        /// </summary>
        /// <param name="inputType">The input type.</param>
        /// <returns>All decorator descriptors for a given input type.</returns>
        public IEnumerable<DecoratorDescriptor> GetDescriptions(Type inputType)
        {
            foreach (var resolver in resolvers)
            {
                var description = resolver.TryResolve(inputType);
                if (description is not null)
                    yield return description;
            }
        }

        /// <summary>
        /// Gets all descriptors for a given input and output types.
        /// </summary>
        /// <param name="inputType">The input type.</param>
        /// <param name="outpuType">The output type.</param>
        /// <returns>All descriptors for a given input and output types.</returns>
        public IEnumerable<DecoratorDescriptor> GetDescriptions(Type inputType, Type outpuType)
        {
            foreach (var resolver in resolvers)
            {
                var description = resolver.TryResolve(inputType, outpuType);
                if (description is not null)
                    yield return description;
            }
        }
    }
}
