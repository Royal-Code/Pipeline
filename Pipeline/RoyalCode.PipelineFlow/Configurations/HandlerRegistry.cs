using RoyalCode.PipelineFlow.Descriptors;
using RoyalCode.PipelineFlow.Exceptions;
using RoyalCode.PipelineFlow.Resolvers;
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

        /// <summary>
        ///     Gets the descriptor for a given input type.
        /// </summary>
        /// <param name="inputType">The input type.</param>
        /// <returns>
        ///     The handler descriptor for a given input type, or null if none was found.
        /// </returns>
        /// <exception cref="MultipleHandlersForTheSameRequestException">
        ///     When there are multiple handlers for one request type (input/output).
        /// </exception>
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
                    continue;
                }

                var resolvedDescription = resolver.TryResolve(inputType);
                if (resolvedDescription is null) 
                    continue;
                
                if (description is not null)
                    throw new MultipleHandlersForTheSameRequestException(inputType);
                
                description = resolvedDescription;
            }

            if (description is not null || fallbackResolvers is null) 
                return description;
            
            foreach (var resolver in fallbackResolvers)
            {
                description = resolver.TryResolve(inputType);
                if (description is not null)
                    break;
            }
            
            return description;
        }

        /// <summary>
        ///     Gets the descriptor for a given input and output types.
        /// </summary>
        /// <param name="inputType">The input type.</param>
        /// <param name="outputType">The output type.</param>
        /// <returns>
        ///     The handler descriptor for a given input and output types, or null if none was found.
        /// </returns>
        /// <exception cref="MultipleHandlersForTheSameRequestException">
        ///     When there are multiple handlers for one request type (input/output).
        /// </exception>
        public HandlerDescriptor? GetDescription(Type inputType, Type outputType)
        {
            LinkedList<IHandlerResolver>? fallbackResolvers = null;
            HandlerDescriptor? description = null;

            foreach (var resolver in resolvers)
            {
                if (resolver.IsFallback)
                {
                    fallbackResolvers ??= new();
                    fallbackResolvers.AddLast(resolver);
                    continue;
                }

                var resolvedDescription = resolver.TryResolve(inputType, outputType);
                if (resolvedDescription is not null)
                {
                    if (description is not null)
                        throw new MultipleHandlersForTheSameRequestException(inputType, outputType);
                    description = resolvedDescription;
                }
            }

            if (description is null && fallbackResolvers is not null)
            {
                foreach (var resolver in fallbackResolvers)
                {
                    description = resolver.TryResolve(inputType, outputType);
                    if (description is not null)
                        break;
                }
            }

            return description;
        }
    }
}
