using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class MethodBridgeHandlerResolver : HandlerResolverBase
    {
        public MethodBridgeHandlerResolver(MethodInfo methodHandler)
            : base(methodHandler.GetBridgeDescription())
        { }
    }
}
