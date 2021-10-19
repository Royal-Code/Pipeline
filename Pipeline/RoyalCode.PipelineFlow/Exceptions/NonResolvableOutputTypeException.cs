using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for not resolvable generic parameter of the handler delegate output type.
    /// </summary>
    public class NonResolvableOutputTypeException : GenericResolutionException
    {
        private static string CreateMessage(MethodInfo method, Type outputType)
        {
            return "Is not possible to resolve the generic type contained in the output type." +
                $" The output type is '{outputType}', the method is '{method.Name}' of class '{method.DeclaringType.Name}'.";
        }

        /// <summary>
        /// Create a new exception.
        /// </summary>
        /// <param name="method">The handler delegate method.</param>
        /// <param name="outputType">The output type.</param>
        public NonResolvableOutputTypeException(MethodInfo method, Type outputType)
            : base(CreateMessage(method, outputType))
        { }
    }
}
