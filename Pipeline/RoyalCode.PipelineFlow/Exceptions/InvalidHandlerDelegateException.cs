using System;

namespace RoyalCode.PipelineFlow.Exceptions
{
    public class InvalidHandlerDelegateException : InvalidOperationException
    {
        public static string DefaultMessage =
            "Invalid delegate, the handler delegate must have one or two parameters, " +
            "the first will be the input type, and if the delegate is asynchronous, " +
            "there can be a second parameter, which must be of type CancellationToken.";

        public InvalidHandlerDelegateException()
            : base(DefaultMessage)
        { }
    }
}
