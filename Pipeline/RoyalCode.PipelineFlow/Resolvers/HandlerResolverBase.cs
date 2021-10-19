using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    /// <summary>
    /// <para>
    ///     Abstract implementation of <see cref="IHandlerResolver"/>.
    /// </para>
    /// <para>
    ///     The resolver component is used to provide descriptors for handlers when they are able to process the pipeline request.
    /// </para>
    /// </summary>
    public abstract class HandlerResolverBase : IHandlerResolver
    {
        private readonly HandlerDescriptor handlerDescription;

        /// <summary>
        /// Create a new resolver for the <see cref="HandlerDescriptor"/>.
        /// </summary>
        /// <param name="handlerDescription">The <see cref="HandlerDescriptor"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="handlerDescription"/> is null.
        /// </exception>
        protected HandlerResolverBase(HandlerDescriptor handlerDescription)
        {
            this.handlerDescription = handlerDescription ?? throw new ArgumentNullException(nameof(handlerDescription));
        }

        /// <inheritdoc/>
        public bool IsFallback { get; protected set; }

        /// <inheritdoc/>
        public HandlerDescriptor? TryResolve(Type inputType)
        {
            return handlerDescription.Match(inputType)
                ? handlerDescription
                : null;
        }

        /// <inheritdoc/>
        public HandlerDescriptor? TryResolve(Type inputType, Type output)
        {
            return handlerDescription.Match(inputType, output)
                ? handlerDescription
                : null;
        }
    }
}
