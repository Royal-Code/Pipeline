using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    /// <summary>
    /// <para>
    ///     Abstract implementation of <see cref="IDecoratorResolver"/>.
    /// </para>
    /// <para>
    ///     The resolver component is used to provide descriptors for handlers when they are able to process the pipeline request.
    /// </para>
    /// </summary>
    public abstract class DecoratorResolverBase : IDecoratorResolver
    {
        private readonly DecoratorDescriptor decoratorDescription;

        /// <summary>
        /// Create a new resolver for the <see cref="DecoratorDescriptor"/>;
        /// </summary>
        /// <param name="decoratorDescription">The <see cref="DecoratorDescriptor"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="decoratorDescription"/> is null.
        /// </exception>
        protected DecoratorResolverBase(DecoratorDescriptor decoratorDescription)
        {
            this.decoratorDescription = decoratorDescription ?? throw new ArgumentNullException(nameof(decoratorDescription));
        }

        /// <inheritdoc/>
        public DecoratorDescriptor? TryResolve(Type inputType)
        {
            return decoratorDescription.Match(inputType)
                ? decoratorDescription
                : null;
        }

        /// <inheritdoc/>
        public DecoratorDescriptor? TryResolve(Type inputType, Type output)
        {
            return decoratorDescription.Match(inputType, output)
                ? decoratorDescription
                : null;
        }
    }
}
