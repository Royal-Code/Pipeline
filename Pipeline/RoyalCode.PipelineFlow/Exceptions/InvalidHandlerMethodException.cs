using System;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for invalid handler method.
    /// </summary>
    public class InvalidHandlerMethodException : InvalidOperationException
    {
        private const string DefaultMessage =
            "Invalid handler method, the method must have one or two parameters, " +
            "the first will be the input type, and if the method is asynchronous, " +
            "there can be a second parameter, which must be of type CancellationToken.";

        /// <summary>
        /// Create a new exception.
        /// </summary>
        public InvalidHandlerMethodException()
            : base(DefaultMessage)
        { }
    }
}
