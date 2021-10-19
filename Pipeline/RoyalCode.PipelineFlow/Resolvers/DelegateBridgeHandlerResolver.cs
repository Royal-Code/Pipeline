using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    /// <summary>
    /// <para>
    ///     A resolver for bridge handlers created from a delegate.
    /// </para>
    /// <para>
    ///     The resolver component is used to provide descriptors for handlers when they are able to process the pipeline request.
    /// </para>
    /// </summary>
    public class DelegateBridgeHandlerResolver : HandlerResolverBase
    {
        /// <summary>
        /// Create a new resolver from a delegate.
        /// </summary>
        /// <param name="handler">The handler delegate.</param>
        public DelegateBridgeHandlerResolver(Delegate handler)
            : base(handler.GetBridgeDescription())
        { }
    }
}
