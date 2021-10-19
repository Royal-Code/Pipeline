using System;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for invalid handler method.
    /// </summary>
    public class InvalidServiceHandlerDelegateException : InvalidOperationException
    {
        public static string DefaultMessage =
            "Invalid delegate, the delegate service handler must have two or three parameters, " +
            "the first will be the service, the second the input type, and if the delegate is asynchronous, " +
            "there can be a third parameter, which must be of type CancellationToken.";

        /// <summary>
        /// Create a new exception.
        /// </summary>
        public InvalidServiceHandlerDelegateException()
            : base(DefaultMessage)
        { }
    }
}
