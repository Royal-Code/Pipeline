
using Microsoft.Extensions.Logging;

namespace RoyalCode.PipelineFlow.EventDispatcher.Options;

/// <summary>
/// <para>
///     Options for event dispathcer.
/// </para>
/// </summary>
public class EventDispatcherOptions
{
    /// <summary>
    /// The log level used by event dispatcher components.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Debug;
}

