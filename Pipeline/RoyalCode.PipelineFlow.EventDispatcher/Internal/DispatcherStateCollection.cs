using System;
using System.Collections.Concurrent;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     A collection of dispatcher state.
/// </para>
/// <para>
///     This collection stores information to determine whether an event has observers and needs to be dispatched or not.
/// </para>
/// </summary>
public class DispatcherStateCollection
{
    private readonly ConcurrentDictionary<Type, DispatcherState> inCurrentScopeDispatcherStates = new();
    private readonly ConcurrentDictionary<Type, DispatcherState> inSeparetedScopeDispatcherStates = new();

    /// <summary>
    /// Get the state for dispatcher in the current scope.
    /// </summary>
    /// <param name="type">The event type.</param>
    /// <returns>The dispatcher state.</returns>
    public DispatcherState GetInCurrentScope(Type type)
        => inCurrentScopeDispatcherStates.GetOrAdd(type, _ => new DispatcherState());
    
    /// <summary>
    /// Get the state for dispatcher in a separeted new scope.
    /// </summary>
    /// <param name="type">The event type.</param>
    /// <returns>The dispatcher state.</returns>
    public DispatcherState GetInSeparetedScope(Type type)
        => inSeparetedScopeDispatcherStates.GetOrAdd(type, _ => new DispatcherState());
}