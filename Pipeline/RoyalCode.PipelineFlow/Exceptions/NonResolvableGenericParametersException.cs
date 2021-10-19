using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Exceptions
{
    public class NonResolvableGenericParametersException : GenericResolutionException
    {
        private static string CreateMessage(MethodInfo method, Type inputType, Type outputType)
        {
            return $"Is not possible to resolve the generic parameters of the type '{method.DeclaringType.Name}'." +
                $" The input type is '{inputType}', the output type is '{outputType}' and the method is '{method.Name}'.";
        }

        public NonResolvableGenericParametersException(MethodInfo method, Type inputType, Type outputType)
            : base(CreateMessage(method, inputType, outputType))
        { }

    }
}
