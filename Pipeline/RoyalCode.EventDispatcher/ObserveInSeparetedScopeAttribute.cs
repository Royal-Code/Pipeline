
using System;

namespace RoyalCode.EventDispatcher;

/// <summary>
/// <para>
///     Determines that events will be observed in a separeted scope.
/// </para>
/// <para>
///     Corresponds to the <see cref="DispatchStrategy.InSeparetedScope"/>.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ObserveInSeparetedScopeAttribute : Attribute { }