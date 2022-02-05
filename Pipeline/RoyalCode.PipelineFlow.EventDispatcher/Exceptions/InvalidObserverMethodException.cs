using System;

namespace RoyalCode.PipelineFlow.EventDispatcher.Exceptions;

/// <summary>
/// Exception for invalid event observer methods.
/// </summary>
public class InvalidObserverMethodException : InvalidOperationException
{
    /// <summary>
    /// Creates a new exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    public InvalidObserverMethodException(string message) : base(message) { }
}