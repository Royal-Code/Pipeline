namespace RoyalCode.Core.Cqs
{
    /// <summary>
    /// Requesição de comando ou query que produz um resultado.
    /// </summary>
    /// <typeparam name="TResult">Tipo do resultado produzido pelo processamento do comando ou query.</typeparam>
    public interface IRequest<TResult> { }

    /// <summary>
    /// Requesição de comando ou query sem resultado.
    /// </summary>
    public interface IRequest : IRequest<IVoidResult> { }
}
