namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// Base interface for <see cref="IBridge{TRequest, TNextRequest}"/>.
    /// </summary>
    /// <typeparam name="TRequest">Data type of the origin request.</typeparam>
    public interface IBridgeBase<in TRequest> where TRequest : IRequest { }

    /// <summary>
    /// Base interface for <see cref="IBridge{TRequest, TResult, TNextRequest}"/>
    /// or <see cref="IBridge{TRequest, TResult, TNextRequest, TNextResult}"/>.
    /// </summary>
    /// <typeparam name="TRequest">Data type of the origin request.</typeparam>
    /// <typeparam name="TResult">Data type of the new (next) request.</typeparam>
    public interface IBridgeBase<in TRequest, TResult> where TRequest : IRequest<TResult> { }
}
