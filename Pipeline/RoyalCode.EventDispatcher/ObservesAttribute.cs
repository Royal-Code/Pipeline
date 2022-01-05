
using System;

namespace RoyalCode.EventDispatcher;

/// <summary>
/// <para>
///     Attribute for use in methods that observes events.
/// </para>
/// TODO: This class must be moved to a domain centric project, and the dispatch strategy must be reviewed.
/// TODO: The strategy may be EventHandlingContext with values like InSameTransaction, InNewTransaction.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ObservesAttribute : Attribute
{
    public ObservesAttribute(DispatchStrategy strategy = DispatchStrategy.InSeparetedScope)
    {
        Strategy = strategy;
    }

    public DispatchStrategy Strategy { get; }

}
