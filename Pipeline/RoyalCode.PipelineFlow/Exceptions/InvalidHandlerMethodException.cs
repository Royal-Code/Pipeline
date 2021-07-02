using System;

namespace RoyalCode.PipelineFlow.Exceptions
{
    public class InvalidHandlerMethodException : InvalidOperationException
    {
        public static string DefaultMessage =
            "Invalid handler method, the method must have one or two parameters, " +
            "the first will be the input type, and if the method is asynchronous, " +
            "there can be a second parameter, which must be of type CancellationToken.";

        public InvalidHandlerMethodException()
            : base(DefaultMessage)
        { }
    }
}
