using RoyalCode.PipelineFlow.Descriptors;
using System;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// <para>
    ///     This class work like a collection to store <see cref="IHandlerResolver"/> and retrieve <see cref="HandlerDescriptor"/>.
    /// </para>
    /// <para>
    ///     The <see cref="IPipelineConfiguration"/> holds the <see cref="HandlerRegistry"/> and the <see cref="DecoratorRegistry"/>
    ///     that contains all the handlers and decorators for building pipelines for a "bus" component kind.
    /// </para>
    /// <para>
    ///     Each kind of "bus" component may have its own policies and restrictions on how to build pipelines.
    /// </para>
    /// </summary>
    public class HandlerRegistry
    {
        private readonly ICollection<IHandlerResolver> resolvers = new List<IHandlerResolver>();

        /// <summary>
        /// Add a <see cref="IHandlerResolver"/>.
        /// </summary>
        /// <param name="handlerResolver">Some <see cref="IHandlerResolver"/>.</param>
        public void Add(IHandlerResolver handlerResolver)
        {
            resolvers.Add(handlerResolver);
        }

        public HandlerDescriptor? GetDescription(Type inputType)
        {
            LinkedList<IHandlerResolver>? fallbackResolvers = null;
            HandlerDescriptor? description = null;

            foreach (var resolver in resolvers)
            {
                if (resolver.IsFallback)
                {
                    fallbackResolvers ??= new();
                    fallbackResolvers.AddLast(resolver);
                }

                var resolvedDescription = resolver.TryResolve(inputType);
                if (resolvedDescription is not null)
                {
                    if (description is not null)
                        throw new InvalidOperationException("Não pode haver mais de um handler. TODO: Criar exeption deste erro.");
                    description = resolvedDescription;
                }
            }

            if (description is null && fallbackResolvers is not null)
            {
                foreach (var resolver in fallbackResolvers)
                {
                    description = resolver.TryResolve(inputType);
                    if (description is not null)
                        break;
                }
            }

            return description;
        }

        public HandlerDescriptor? GetDescription(Type inputType, Type output)
        {
            LinkedList<IHandlerResolver>? fallbackResolvers = null;
            HandlerDescriptor? description = null;

            foreach (var resolver in resolvers)
            {
                if (resolver.IsFallback)
                {
                    fallbackResolvers ??= new();
                    fallbackResolvers.AddLast(resolver);
                }

                var resolvedDescription = resolver.TryResolve(inputType, output);
                if (resolvedDescription is not null)
                {
                    if (description is not null)
                        throw new InvalidOperationException("Não pode haver mais de um handler. TODO: Criar exeption deste erro.");
                    description = resolvedDescription;
                }
            }

            if (description is null && fallbackResolvers is not null)
            {
                foreach (var resolver in fallbackResolvers)
                {
                    description = resolver.TryResolve(inputType, output);
                    if (description is not null)
                        break;
                }
            }

            return description;
        }
    }
}
