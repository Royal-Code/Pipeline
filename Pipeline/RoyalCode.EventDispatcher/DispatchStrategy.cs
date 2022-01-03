namespace RoyalCode.EventDispatcher;

/// <summary>
/// <para>
///     Strategy to dispatch events, in the current scope, or in a separeted scope.
/// </para>
/// </summary>
public enum DispatchStrategy
{
    /// <summary>
    /// This type of dispatch must be done in the same work unit, transaction.
    /// </summary>
    InCurrentScope,

    /// <summary>
    /// This type of dispatch should be made after the end of the work unit, 
    /// after the transaction has been successfully commited.
    /// </summary>
    InSeparetedScope,
}
