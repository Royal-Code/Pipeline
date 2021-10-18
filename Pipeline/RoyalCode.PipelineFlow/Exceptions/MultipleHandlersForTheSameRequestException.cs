using System;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for when there are multiple handlers for one request type (input/output).
    /// </summary>
    public class MultipleHandlersForTheSameRequestException : InvalidOperationException
    {
        private const string BaseMessage = "There cannot be more than one handler for the same type of request (input/output).";
        private const string InputComplementPattern = "The input type is {0}.";
        private const string InputAndOutpuComplementPattern = "The input type is {0} and the output type is {1}.";

        private static string CreateMessage(Type inputType, Type? outputType)
            => outputType is null
                ? $"{BaseMessage} {string.Format(InputComplementPattern, inputType.Name)}"
                : $"{BaseMessage} {string.Format(InputAndOutpuComplementPattern, inputType.Name, outputType.Name)}";

        /// <summary>
        /// Creates a new instace of the exception.
        /// </summary>
        /// <param name="inputType">The input type.</param>
        /// <param name="outputType">The output type.</param>
        public MultipleHandlersForTheSameRequestException(Type inputType, Type? outputType = null)
            : base(CreateMessage(inputType, outputType))
        { }
    }
}
