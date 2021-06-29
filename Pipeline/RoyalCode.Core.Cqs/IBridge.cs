using System;

namespace RoyalCode.Core.Cqs
{
    /// <summary>
    /// Handler intermediário, o qual redirecionará a request a outro handler.
    /// </summary>
    /// <typeparam name="TRequest">A Request original.</typeparam>
    /// <typeparam name="TNextRequest">A nova request, a qual deve ter o mesmo resultado.</typeparam>
    public interface IBridge<in TRequest, TNextRequest> : IBridgeBase<TRequest>
        where TRequest : IRequest
        where TNextRequest : IRequest
    {
        /// <summary>
        /// Obtém a nova request a partir da request original.
        /// </summary>
        /// <param name="request">Request Original.</param>
        /// <param name="next">Delegate for the next action in the pipeline.</param>
        void Next(TRequest request, Action<TNextRequest> next);
    }

    /// <summary>
    /// Handler intermediário, o qual redirecionará a request a outro handler.
    /// </summary>
    /// <typeparam name="TRequest">A Request original.</typeparam>
    /// <typeparam name="TResult">O Resultado da Request.</typeparam>
    /// <typeparam name="TNextRequest">A nova request, a qual deve ter o mesmo resultado.</typeparam>
    public interface IBridge<in TRequest, TResult, TNextRequest> : IBridgeBase<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TNextRequest : IRequest<TResult>
    {
        /// <summary>
        /// Obtém a nova request a partir da request original.
        /// </summary>
        /// <param name="request">Request Original.</param>
        /// <param name="next">Delegate for the next action in the pipeline.</param>
        /// <returns>O Resultado da execução da requisição.</returns>
        TResult Next(TRequest request, Func<TNextRequest, TResult> next);
    }

    /// <summary>
    /// Handler intermediário, o qual redirecionará a request a outro handler.
    /// </summary>
    /// <typeparam name="TRequest">A Request original.</typeparam>
    /// <typeparam name="TResult">O Resultado da Request.</typeparam>
    /// <typeparam name="TNextRequest">A nova request, a qual deve ter o mesmo resultado.</typeparam>
    public interface IBridge<in TRequest, TResult, TNextRequest, TNextResult> : IBridgeBase<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TNextRequest : IRequest<TNextResult>
    {
        /// <summary>
        /// Obtém a nova request a partir da request original.
        /// </summary>
        /// <param name="request">Request Original.</param>
        /// <param name="next">Delegate for the next action in the pipeline.</param>
        /// <returns>O Resultado da execução da requisição.</returns>
        TResult Next(TRequest request, Func<TNextRequest, TNextResult> next);
    }
}
