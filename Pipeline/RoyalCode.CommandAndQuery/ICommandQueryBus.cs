using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// <para>
    ///     Commands and Queries bus component.
    ///     Command and query in the sense of CQS.
    /// </para>
    /// <para>
    ///     This is an implementation of the command-handler-pattern including decorators and bridges between requests.
    /// </para>
    /// <para>
    ///     Queries and commands sent over the bus are treated as requests (<see cref="IRequest"/>).
    /// </para>
    /// <para>
    ///     The bus acts as a mediator between the request (command or query) 
    ///     and the handler (<see cref="IHandler{TRequest}"/> and others).
    /// </para>
    /// </summary>
    public interface ICommandQueryBus
    {
        /// <summary>
        /// Sends a request across the bus to be processed.
        /// </summary>
        /// <param name="request">The request (command or query).</param>
        void Send(IRequest request);

        /// <summary>
        /// Sends a request through the bus to be processed asynchronously.
        /// </summary>
        /// <param name="request">The request (command or query).</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Task for asynchronous processing.</returns>
        Task SendAsync(IRequest request, CancellationToken token = default);

        /// <summary>
        /// Sends a request through the bus to be processed, returning the result produced.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="request">The request (command or query).</param>
        /// <returns>Request Result.</returns>
        TResult Send<TResult>(IRequest<TResult> request);

        /// <summary>
        /// Sends a request through the bus to be processed asynchronously, returning the result produced.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="request">The request (command or query).</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Task for asynchronous processing with the result.</returns>
        Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token = default);
    }
}
