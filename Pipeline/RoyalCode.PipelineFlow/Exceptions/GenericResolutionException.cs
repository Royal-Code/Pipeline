using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for <see cref="GenericResolution"/> class.
    /// </summary>
    public abstract class GenericResolutionException : InvalidOperationException
    {
        /// <summary>
        /// Create a new exception with the cause message.
        /// </summary>
        /// <param name="message">The cause message.</param>
        protected internal GenericResolutionException(string message) : base(message) { }
    }
}
