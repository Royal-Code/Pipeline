using RoyalCode.PipelineFlow.Descriptors;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Resolvers
{
    /// <para>
    ///     A resolver for processing handler created from a method.
    /// </para>
    /// <para>
    ///     The resolver component is used to provide descriptors for handlers when they are able to process the pipeline request.
    /// </para>
    public class MethodHandlerResolver : HandlerResolverBase
    {
        /// <summary>
        /// Create a new resolver from a method.
        /// </summary>
        /// <param name="methodHandler">The method handler.</param>
        public MethodHandlerResolver(MethodInfo methodHandler)
            : base(methodHandler.GetHandlerDescription())
        { }
    }
}
