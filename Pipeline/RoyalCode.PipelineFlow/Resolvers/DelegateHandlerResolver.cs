using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    /// <para>
    ///     A resolver for processing handler created from a delegate.
    /// </para>
    /// <para>
    ///     The resolver component is used to provide descriptors for handlers when they are able to process the pipeline request.
    /// </para>
    public class DelegateHandlerResolver : HandlerResolverBase
    {
        /// <summary>
        /// Create a new resolver from a delegate.
        /// </summary>
        /// <param name="handler">The handler delegate.</param>
        public DelegateHandlerResolver(Delegate handler)
            : base(handler.GetHandlerDescription())
        { }
    }
}
