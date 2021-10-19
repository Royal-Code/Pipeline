using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Exceptions
{
    /// <summary>
    /// Exception for <see cref="GenericResolution"/> class.
    /// </summary>
    public abstract class GenericResolutionException : InvalidOperationException
    {
        protected internal GenericResolutionException(string message) : base(message) { }
    }
}
