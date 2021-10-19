using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Exceptions
{
    public class NonResolvableOutputTypeException : GenericResolutionException
    {
        private static string CreateMessage(MethodInfo method, Type outputType)
        {
            return "Is not possible to resolve the generic type contained in the output type." +
                $" The output type is '{outputType}', the method is '{method.Name}' of class '{method.DeclaringType.Name}'.";
        }

        public NonResolvableOutputTypeException(MethodInfo method, Type outputType)
            : base(CreateMessage(method, outputType))
        { }
    }
}
