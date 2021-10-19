using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for not resolvable generic parameters of handlers delegates.
    /// </summary>
    public class NonResolvableGenericParametersException : GenericResolutionException
    {
        private static string CreateMessage(MethodInfo method, Type inputType, Type outputType)
        {
            return $"Is not possible to resolve the generic parameters of the type '{method.DeclaringType.Name}'." +
                $" The input type is '{inputType}', the output type is '{outputType}' and the method is '{method.Name}'.";
        }

        /// <summary>
        /// Create a new exception.
        /// </summary>
        /// <param name="method">The handler delegate method.</param>
        /// <param name="inputType">The input type.</param>
        /// <param name="outputType">The output type.</param>
        public NonResolvableGenericParametersException(MethodInfo method, Type inputType, Type outputType)
            : base(CreateMessage(method, inputType, outputType))
        { }
    }
}
