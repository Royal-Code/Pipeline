﻿
using System;

namespace RoyalCode.EventDispatcher;

/// <summary>
/// <para>
///     Determines that events will be observed in the current scope.
/// </para>
/// <para>
///     Corresponds to the <see cref="DispatchStrategy.InCurrentScope"/>.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ObserveInCurrentScopeAttribute : Attribute { }
