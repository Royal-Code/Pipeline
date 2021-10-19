using System;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for invalid decorator handler delegate.
    /// </summary>
    public class InvalidDecoratorDelegateException : InvalidOperationException
    {
        public static string DefaultMessage =
            "Invalid delegate, the decorator handler delegate must have two or three parameters, " +
            "the first will be the input type, the second will be the next hander, " +
            "and if the delegate is asynchronous, there can be a thrid parameter, " +
            "which must be of type CancellationToken.";

        /// <summary>
        /// Create a new exception.
        /// </summary>
        public InvalidDecoratorDelegateException()
            : base(DefaultMessage)
        { }
    }
}
