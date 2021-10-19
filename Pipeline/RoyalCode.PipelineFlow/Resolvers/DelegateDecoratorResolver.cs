using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    /// <para>
    ///     A resolver for decorator handler created from a delegate.
    /// </para>
    /// <para>
    ///     The resolver component is used to provide descriptors for handlers when they are able to process the pipeline request.
    /// </para>
    public class DelegateDecoratorResolver : DecoratorResolverBase
    {
        /// <summary>
        /// Create a new resolver from a delegate.
        /// </summary>
        /// <param name="decoratorHandler">The handler delegate.</param>
        public DelegateDecoratorResolver(Delegate decoratorHandler)
            : base(decoratorHandler.GetDecoratorDescription())
        { }
    }
}
