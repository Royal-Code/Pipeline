using System;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class HandlerRegistry
    {
        private readonly ICollection<IHandlerResolver> resolvers = new List<IHandlerResolver>();

        public void Add(IHandlerResolver handlerResolver)
        {
            resolvers.Add(handlerResolver);
        }

        public HandlerDescription? GetDescription(Type inputType)
        {
            LinkedList<IHandlerResolver>? fallbackResolvers = null;
            HandlerDescription? description = null;

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
                        throw new InvalidOperationException("Não pode haver mais de um handler");
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

        public HandlerDescription? GetDescription(Type inputType, Type output)
        {
            LinkedList<IHandlerResolver>? fallbackResolvers = null;
            HandlerDescription? description = null;

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
                        throw new InvalidOperationException("Não pode haver mais de um handler");
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
