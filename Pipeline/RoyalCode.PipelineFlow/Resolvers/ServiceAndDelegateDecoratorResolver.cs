using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    public class ServiceAndDelegateDecoratorResolver : DecoratorResolverBase
    {
        public ServiceAndDelegateDecoratorResolver(Delegate decoratorHandler, Type serviceType)
            : base(decoratorHandler.GetDecoratorDescription(serviceType))
        { }
    }
}
