using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// <para>
    ///     Manipulador (Handler) de requisições de um pipeline.
    ///     O Handler executa, processa, uma requisição.
    /// </para>
    /// <para>
    ///     Este componente é parte do Design Pattern Command e Mediator, intrínseco ao CQS - CQRS.
    ///     Uma request de comando é enviado para o mediador (<see cref="ICommandQueryBus"/>), 
    ///     passa por um pipeline e chega ao manipulador, que é esta interface.
    /// </para>
    /// <para>
    ///     Este manipulador processa a requisição e não retorna nenhum resultado.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Tipo de dado da requisição.</typeparam>
    public interface IAsyncHandler<in TRequest>
        where TRequest : IRequest
    {
        /// <summary>
        /// Processa, executa, a requisição.
        /// </summary>
        /// <param name="request">Requisição para execução.</param>
        /// <param name="token">Token para cancelamento da requisição.</param>
        /// <returns>A tarefa assíncrona da execução.</returns>
        Task HandleAsync(TRequest request, CancellationToken token = default);
    }

    /// <summary>
    /// <para>
    ///     Manipulador (Handler) de requisições de um pipeline.
    /// </para>
    /// <para>
    ///     O Handler executa, processa, uma requisição.
    /// </para>
    /// <para>
    ///     Este componente é parte do Design Pattern Command e Mediator, intrínseco ao CQS - CQRS.
    ///     Uma request de comando é enviado para o mediador (<see cref="ICommandQueryBus"/>), 
    ///     passa por um pipeline e chega ao manipulador, que é esta interface.
    /// </para>
    /// <para>
    ///     Este manipulador processa a requisição e retorna resultado requerido.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Tipo de dado da requisição.</typeparam>
    /// <typeparam name="TResult">Tipo de dado do resultado.</typeparam>
    public interface IAsyncHandler<in TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        /// <summary>
        /// Processa, executa, a requisição produzindo um resultado.
        /// </summary>
        /// <param name="request">Requisição para execução.</param>
        /// <param name="token">Token para cancelamento da requisição.</param>
        /// <returns>A tarefa assíncrona da execução.</returns>
        Task<TResult> HandleAsync(TRequest request, CancellationToken token = default);
    }
}
