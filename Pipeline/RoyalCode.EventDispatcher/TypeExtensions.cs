using System;
using System.Linq;
using System.Reflection;

namespace RoyalCode.EventDispatcher;

/// <summary>
/// Extension methods for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// <para>
    ///     Given a type, the attributes are checked to determine the dispatch strategy.
    /// </para>
    /// </summary>
    /// <param name="type">Some observer type.</param>
    /// <returns>The dispatcher type.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para>
    ///     If the <paramref name="type"/> is null.
    /// </para>
    /// </exception>
    public static DispatchStrategy GetDispatchStrategy(this Type type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        return type.GetCustomAttributes<ObserveInCurrentScopeAttribute>(true).Any()
            ? DispatchStrategy.InCurrentScope
            : DispatchStrategy.InSeparetedScope;
    }
}

