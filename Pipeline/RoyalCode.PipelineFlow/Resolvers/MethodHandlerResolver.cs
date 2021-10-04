using RoyalCode.PipelineFlow.Descriptors;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Resolvers
{
    public class MethodHandlerResolver : HandlerResolverBase
    {
        public MethodHandlerResolver(MethodInfo methodHandler)
            : base(methodHandler.GetHandlerDescription())
        { }
    }
}
