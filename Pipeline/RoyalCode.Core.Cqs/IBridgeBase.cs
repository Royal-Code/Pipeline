namespace RoyalCode.Core.Cqs
{
    /// <summary>
    /// Interface base para <see cref="IBridge{TRequest, TResult, TNextRequest}"/> para ser obtida
    /// por injeção de dependência.
    /// </summary>
    /// <typeparam name="TRequest">A Request original.</typeparam>
    /// <typeparam name="TResult">O Resultado da Request.</typeparam>
    public interface IBridgeBase<in TRequest, TResult> where TRequest : IRequest<TResult> { }

    /// <summary>
    /// Interface base para <see cref="IBridge{TRequest, TNextRequest}"/> para ser obtida
    /// por injeção de dependência.
    /// </summary>
    /// <typeparam name="TRequest">A Request original.</typeparam>
    public interface IBridgeBase<in TRequest> where TRequest : IRequest { }
}
