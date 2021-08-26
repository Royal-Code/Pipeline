using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class ServiceAndDelegateDecoratorResolver : DecoratorResolverBase
    {
        public ServiceAndDelegateDecoratorResolver(Delegate decoratorHandler, Type serviceType)
            : base(decoratorHandler.GetDecoratorDescription(serviceType))
        { }
    }
}
