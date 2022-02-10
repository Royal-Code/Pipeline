using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// <para>
    ///     Decorators for pipeline requests.
    ///     These objects are executed by the <see cref="ICommandQueryBus"/>.
    /// </para>
    /// <para>
    ///     The decorator adds a behavior to the pipeline requests,
    ///     it may modify the input value, perform some operation, 
    ///     stop or break the pipeline execution, or return a result value for request.
    /// </para>
    /// <para>
    ///     After the decorator behavior has been performed, the next handler should be called, 
    ///     otherwise execution will be interrupted.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Data type of the request.</typeparam>
    public interface IAsyncDecorator<in TRequest>
    {
        /// <summary>
        /// Pipeline decorator handler.
        /// Perform any additional behavior and await the <paramref name="next"/> delegate as necessary.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="next">
        ///     Awaitable delegate for the next action in the pipeline.
        /// </param>
        /// <returns>
        ///     Task for asynchronous processing.
        /// </returns>
        Task HandleAsync(TRequest request, Func<Task> next, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// <para>
    ///     Decorators for pipeline requests.
    ///     These objects are executed by the <see cref="ICommandQueryBus"/>.
    /// </para>
    /// <para>
    ///     The decorator adds a behavior to the pipeline requests,
    ///     it may modify the input value, perform some operation, 
    ///     stop or break the pipeline execution, or return a result value for request.
    /// </para>
    /// <para>
    ///     After the decorator behavior has been performed, the next handler should be called, 
    ///     otherwise execution will be interrupted.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Data type of the request.</typeparam>
    /// <typeparam name="TResult">Result date type.</typeparam>
    public interface IAsyncDecorator<in TRequest, TResult>
    {
        /// <summary>
        /// Pipeline decorator handler.
        /// Perform any additional behavior and await the <paramref name="next"/> delegate as necessary.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="next">
        ///     Awaitable delegate for the next action in the pipeline.
        /// </param>
        /// <returns>
        ///     ask for asynchronous processing with the result of the request execution.
        /// </returns>
        Task<TResult> HandleAsync(TRequest request, Func<Task<TResult>> next, CancellationToken cancellationToken = default);
    }
}
