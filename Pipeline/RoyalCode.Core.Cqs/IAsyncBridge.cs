using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.Core.Cqs
{
    /// <summary>
    /// Handler intermediário, o qual redirecionará a request a outro handler.
    /// </summary>
    /// <typeparam name="TRequest">A Request original.</typeparam>
    /// <typeparam name="TNextRequest">A nova request, a qual deve ter o mesmo resultado.</typeparam>
    public interface IAsyncBridge<in TRequest, TNextRequest> : IBridgeBase<TRequest>
        where TRequest : IRequest
        where TNextRequest : IRequest
    {
        /// <summary>
        /// Obtém a nova request a partir da request original.
        /// </summary>
        /// <param name="request">Request Original.</param>
        /// <param name="next">Delegate for the next action in the pipeline.</param>
        /// <param name="token">Token para cancelamento da tarefa.</param>
        /// <returns>A tarefa assíncrona da execução.</returns>
        Task NextAsync(TRequest request, Func<TNextRequest, Task> next, CancellationToken token = default);
    }

    /// <summary>
    /// Handler intermediário, o qual redirecionará a request a outro handler.
    /// </summary>
    /// <typeparam name="TRequest">A Request original.</typeparam>
    /// <typeparam name="TResult">O Resultado da Request.</typeparam>
    /// <typeparam name="TNextRequest">A nova request, a qual deve ter o mesmo resultado.</typeparam>
    public interface IAsyncBridge<in TRequest, TResult, TNextRequest> : IBridgeBase<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TNextRequest : IRequest<TResult>
    {
        /// <summary>
        /// Obtém a nova request a partir da request original.
        /// </summary>
        /// <param name="request">Request Original.</param>
        /// <param name="next">Delegate for the next action in the pipeline.</param>
        /// <param name="token">Token para cancelamento da tarefa.</param>
        /// <returns>A tarefa assíncrona da execução.</returns>
        Task<TResult> NextAsync(TRequest request, Func<TNextRequest, Task<TResult>> next, CancellationToken token = default);
    }

    /// <summary>
    /// Handler intermediário, o qual redirecionará a request a outro handler.
    /// </summary>
    /// <typeparam name="TRequest">A Request original.</typeparam>
    /// <typeparam name="TResult">O Resultado da Request.</typeparam>
    /// <typeparam name="TNextRequest">A nova request, a qual deve ter o mesmo resultado.</typeparam>
    public interface IAsyncBridge<in TRequest, TResult, TNextRequest, TNextResult> : IBridgeBase<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TNextRequest : IRequest<TNextResult>
    {
        /// <summary>
        /// Obtém a nova request a partir da request original.
        /// </summary>
        /// <param name="request">Request Original.</param>
        /// <param name="next">Delegate for the next action in the pipeline.</param>
        /// <param name="token">Token para cancelamento da tarefa.</param>
        /// <returns>A tarefa assíncrona da execução.</returns>
        Task<TResult> NextAsync(TRequest request, Func<TNextRequest, Task<TNextResult>> next, CancellationToken token = default);
    }
}
