using RoyalCode.PipelineFlow.Descriptors;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Resolvers
{
    public class MethodBridgeHandlerResolver : HandlerResolverBase
    {
        public MethodBridgeHandlerResolver(MethodInfo methodHandler)
            : base(methodHandler.GetBridgeDescription())
        { }
    }
}
