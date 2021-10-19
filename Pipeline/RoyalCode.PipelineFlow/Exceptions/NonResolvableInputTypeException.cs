using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for not resolvable generic parameter of the handler delegate input type.
    /// </summary>
    public class NonResolvableInputTypeException : GenericResolutionException
    {
        private static string CreateMessage(MethodInfo method, Type inputType)
        {
            return "Is not possible to resolve the generic type contained in the input type." +
                $" The input type is '{inputType}', the method is '{method.Name}' of class '{method.DeclaringType.Name}'.";
        }

        /// <summary>
        /// Create a new exception.
        /// </summary>
        /// <param name="method">The handler delegate method.</param>
        /// <param name="inputType">The input type.</param>
        public NonResolvableInputTypeException(MethodInfo method, Type inputType) 
            : base(CreateMessage(method, inputType))
        { }
    }
}
