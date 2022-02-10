using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// Intermediate handler, which will redirect the request to another handler, creating a new request for it.
    /// This handler acts as a bridge between one request (command or query) and another.
    /// </summary>
    /// <typeparam name="TRequest">Data type of the origin request.</typeparam>
    /// <typeparam name="TNextRequest">Data type of the new (next) request.</typeparam>
    public interface IAsyncBridge<in TRequest, TNextRequest> : IBridgeBase<TRequest>
        where TRequest : IRequest
        where TNextRequest : IRequest
    {
        /// <summary>
        /// Handle the origin request, creates the new (next) request and passes it forward in the execution pipeline.
        /// </summary>
        /// <param name="request">The origin request.</param>
        /// <param name="next">Delegate for the next handle in the pipeline.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Task for asynchronous processing.</returns>
        Task NextAsync(TRequest request, Func<TNextRequest, Task> next, CancellationToken token = default);
    }

    /// <summary>
    /// Intermediate handler, which will redirect the request to another handler, creating a new request for it.
    /// This handler acts as a bridge between one request (command or query) and another.
    /// </summary>
    /// <typeparam name="TRequest">Data type of the origin request.</typeparam>
    /// <typeparam name="TResult">Result date type.</typeparam>
    /// <typeparam name="TNextRequest">Data type of the new (next) request.</typeparam>
    public interface IAsyncBridge<in TRequest, TResult, TNextRequest> : IBridgeBase<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TNextRequest : IRequest<TResult>
    {
        /// <summary>
        /// Handle the origin request, creates the new (next) request and passes it forward in the execution pipeline.
        /// </summary>
        /// <param name="request">The origin request.</param>
        /// <param name="next">Delegate for the next handle in the pipeline.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Task for asynchronous processing with the result of the request execution.</returns>
        Task<TResult> NextAsync(TRequest request, Func<TNextRequest, Task<TResult>> next, CancellationToken token = default);
    }

    /// <summary>
    /// Intermediate handler, which will redirect the request to another handler, creating a new request for it.
    /// This handler acts as a bridge between one request (command or query) and another.
    /// </summary>
    /// <typeparam name="TRequest">Data type of the origin request.</typeparam>
    /// <typeparam name="TResult">Result date type.</typeparam>
    /// <typeparam name="TNextRequest">Data type of the new (next) request.</typeparam>
    /// <typeparam name="TNextResult">Data type of the new (next) result.</typeparam>
    public interface IAsyncBridge<in TRequest, TResult, TNextRequest, TNextResult> : IBridgeBase<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TNextRequest : IRequest<TNextResult>
    {
        /// <summary>
        /// Handle the origin request, creates the new (next) request and passes it forward in the execution pipeline.
        /// </summary>
        /// <param name="request">The origin request.</param>
        /// <param name="next">Delegate for the next handle in the pipeline.</param>
        /// <param name="token">Cancellation Token.</param>
        /// <returns>Task for asynchronous processing with the result of the request execution.</returns>
        Task<TResult> NextAsync(TRequest request, Func<TNextRequest, Task<TNextResult>> next, CancellationToken token = default);
    }
}
