using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// <para>
    ///     Handles the request asynchronously.
    /// </para>
    /// <para>
    ///     A request is a command or query, in the CQS sense.
    /// </para>
    /// <para>
    ///     This handler is for requests that do not produce any results.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Data type of the request.</typeparam>
    public interface IAsyncHandler<in TRequest>
        where TRequest : IRequest
    {
        /// <summary>
        /// Processes, executes, the request.
        /// </summary>
        /// <param name="request">The request (command or query) for execution.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Task for asynchronous processing.</returns>
        Task HandleAsync(TRequest request, CancellationToken token = default);
    }

    /// <summary>
    /// <para>
    ///     Handles the request asynchronously.
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
    public interface IAsyncHandler<in TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        /// <summary>
        /// Processes, executes, the request producing a result.
        /// </summary>
        /// <param name="request">The request (command or query) for execution.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Task for asynchronous processing with the result of the request execution.</returns>
        Task<TResult> HandleAsync(TRequest request, CancellationToken token = default);
    }
}
