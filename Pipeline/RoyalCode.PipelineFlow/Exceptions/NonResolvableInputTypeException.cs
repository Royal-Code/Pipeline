using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Exceptions
{
    public class NonResolvableInputTypeException : GenericResolutionException
    {
        private static string CreateMessage(MethodInfo method, Type inputType)
        {
            return "Is not possible to resolve the generic type contained in the input type." +
                $" The input type is '{inputType}', the method is '{method.Name}' of class '{method.DeclaringType.Name}'.";
        }

        public NonResolvableInputTypeException(MethodInfo method, Type inputType) 
            : base(CreateMessage(method, inputType))
        { }
    }
}
