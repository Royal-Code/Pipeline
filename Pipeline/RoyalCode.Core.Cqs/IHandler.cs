namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// <para>
    ///     Request handler.
    /// </para>
    /// <para>
    ///     A request is a command or query, in the CQS sense.
    /// </para>
    /// <para>
    ///     This handler is for requests that do not produce any results.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Data type of the request.</typeparam>
    public interface IHandler<in TRequest>
        where TRequest : IRequest
    {
        /// <summary>
        /// Processes, executes, the request.
        /// </summary>
        /// <param name="request">The request (command or query) for execution.</param>
        void Handle(TRequest request);
    }

    /// <summary>
    /// <para>
    ///     Request handler.
    /// </para>
    /// <para>
    ///     A request is a command or query, in the CQS sense.
    /// </para>
    /// <para>
    ///     This handler processes the request and returns the required result.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Data type of the request.</typeparam>
    /// <typeparam name="TResult">Result data type.</typeparam>
    public interface IHandler<in TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        /// <summary>
        /// Processes, executes, the request producing a result.
        /// </summary>
        /// <param name="request">The request (command or query) for execution.</param>
        /// <returns>The result of the request execution.</returns>
        TResult Handle(TRequest request);
    }
}
