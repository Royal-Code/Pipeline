using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class MethodDecoratorResolver : DecoratorResolverBase
    {
        public MethodDecoratorResolver(MethodInfo methodHandler)
            : base(methodHandler.GetDecoratorDescription())
        { }
    }
}
