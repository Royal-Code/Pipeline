using System;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for invalid handler delegate.
    /// </summary>
    public class InvalidHandlerDelegateException : InvalidOperationException
    {
        private const string DefaultMessage =
            "Invalid delegate, the handler delegate must have one or two parameters, " +
            "the first will be the input type, and if the delegate is asynchronous, " +
            "there can be a second parameter, which must be of type CancellationToken.";

        /// <summary>
        /// Create a new exception.
        /// </summary>
        public InvalidHandlerDelegateException()
            : base(DefaultMessage)
        { }
    }
}
