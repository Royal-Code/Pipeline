using System;

namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// Intermediate handler, which will redirect the request to another handler, creating a new request for it.
    /// This handler acts as a bridge between one request (command or query) and another.
    /// </summary>
    /// <typeparam name="TRequest">Data type of the origin request.</typeparam>
    /// <typeparam name="TNextRequest">Data type of the new (next) request.</typeparam>
    public interface IBridge<in TRequest, TNextRequest> : IBridgeBase<TRequest>
        where TRequest : IRequest
        where TNextRequest : IRequest
    {
        /// <summary>
        /// Handle the origin request, creates the new (next) request and passes it forward in the execution pipeline.
        /// </summary>
        /// <param name="request">The origin request.</param>
        /// <param name="next">Delegate for the next handle in the pipeline.</param>
        void Next(TRequest request, Action<TNextRequest> next);
    }

    /// <summary>
    /// Intermediate handler, which will redirect the request to another handler, creating a new request for it.
    /// This handler acts as a bridge between one request (command or query) and another.
    /// </summary>
    /// <typeparam name="TRequest">Data type of the origin request.</typeparam>
    /// <typeparam name="TResult">Result date type.</typeparam>
    /// <typeparam name="TNextRequest">Data type of the new (next) request.</typeparam>
    public interface IBridge<in TRequest, TResult, TNextRequest> : IBridgeBase<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TNextRequest : IRequest<TResult>
    {
        /// <summary>
        /// Handle the origin request, creates the new (next) request and passes it forward in the execution pipeline.
        /// </summary>
        /// <param name="request">The origin request.</param>
        /// <param name="next">Delegate for the next handle in the pipeline.</param>
        /// <returns>The result of the request execution.</returns>
        TResult Next(TRequest request, Func<TNextRequest, TResult> next);
    }

    /// <summary>
    /// Intermediate handler, which will redirect the request to another handler, creating a new request for it.
    /// This handler acts as a bridge between one request (command or query) and another.
    /// </summary>
    /// <typeparam name="TRequest">Data type of the origin request.</typeparam>
    /// <typeparam name="TResult">Result date type.</typeparam>
    /// <typeparam name="TNextRequest">Data type of the new (next) request.</typeparam>
    /// <typeparam name="TNextResult">Data type of the new (next) result.</typeparam>
    public interface IBridge<in TRequest, TResult, TNextRequest, TNextResult> : IBridgeBase<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TNextRequest : IRequest<TNextResult>
    {
        /// <summary>
        /// Handle the origin request, creates the new (next) request and passes it forward in the execution pipeline.
        /// </summary>
        /// <param name="request">The origin request.</param>
        /// <param name="next">Delegate for the next action in the pipeline.</param>
        /// <returns>The result of the request execution.</returns>
        TResult Next(TRequest request, Func<TNextRequest, TNextResult> next);
    }
}
