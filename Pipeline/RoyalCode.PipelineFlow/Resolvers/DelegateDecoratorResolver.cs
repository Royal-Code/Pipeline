using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    public class DelegateDecoratorResolver : DecoratorResolverBase
    {
        public DelegateDecoratorResolver(Delegate decoratorHandler)
            : base(decoratorHandler.GetDecoratorDescription())
        { }
    }
}
