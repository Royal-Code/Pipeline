using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    /// <para>
    ///     A resolver for decorator handler created from a delegate and a service type.
    /// </para>
    /// <para>
    ///     The resolver component is used to provide descriptors for handlers when they are able to process the pipeline request.
    /// </para>
    public class ServiceAndDelegateDecoratorResolver : DecoratorResolverBase
    {
        /// <summary>
        /// Create a new resolver from a delegate and a service type.
        /// </summary>
        /// <param name="decoratorHandler">The handler delegate.</param>
        /// <param name="serviceType">The service type.</param>
        public ServiceAndDelegateDecoratorResolver(Delegate decoratorHandler, Type serviceType)
            : base(decoratorHandler.GetDecoratorDescription(serviceType))
        { }
    }
}
