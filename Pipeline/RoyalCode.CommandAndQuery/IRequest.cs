namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// <para>
    ///     A command or query request that will be handled and produce some result (<typeparamref name="TResult"/>), the response.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of result produced by processing the command or query.</typeparam>
    public interface IRequest<TResult> { }

    /// <summary>
    /// <para>
    ///     A command request that produces no result.
    /// </para>
    /// </summary>
    public interface IRequest : IRequest<INothing> { }
}
