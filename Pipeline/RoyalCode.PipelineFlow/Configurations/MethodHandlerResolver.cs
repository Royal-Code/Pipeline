using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class MethodHandlerResolver : HandlerResolverBase
    {
        public MethodHandlerResolver(MethodInfo methodHandler)
            : base(methodHandler.GetHandlerDescription())
        { }
    }
}
