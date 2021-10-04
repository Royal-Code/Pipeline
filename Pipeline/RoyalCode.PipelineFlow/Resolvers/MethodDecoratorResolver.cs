using RoyalCode.PipelineFlow.Descriptors;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Resolvers
{
    public class MethodDecoratorResolver : DecoratorResolverBase
    {
        public MethodDecoratorResolver(MethodInfo methodHandler)
            : base(methodHandler.GetDecoratorDescription())
        { }
    }
}
