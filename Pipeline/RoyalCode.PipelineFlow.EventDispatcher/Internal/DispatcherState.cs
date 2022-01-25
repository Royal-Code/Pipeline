namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     Has information about the dispatcher of an event type.
/// </para>
/// </summary>
public class DispatcherState
{
    /// <summary>
    /// Determines if the event type has observers.
    /// </summary>
    public bool HasObservers { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="DispatcherState"/>.
    /// </summary>
    public DispatcherState()
    {
        HasObservers = true;
    }
}