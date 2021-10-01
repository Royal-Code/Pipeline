using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// <para>
    ///     Decoradores para requisições do pipeline.
    ///     Estes objetos são executados pelo <see cref="ICommandQueryBus"/>.
    /// </para>
    /// <para>
    ///     O decorador adiciona um comportamento às requisições do pipeline, podendo modificar o valor de entrada 
    ///     ou executar alguma operação.
    /// </para>
    /// <para>
    ///     Após realizado o comportamento do decorador, deve ser chamado o próximo handler, caso contrário a 
    ///     execução será interrompida.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Tipo de dado da requisição.</typeparam>
    public interface IAsyncDecorator<in TRequest>
    {
        /// <summary>
        /// Pipeline decorator.
        /// Perform any additional behavior and await the <paramref name="next"/> delegate as necessary.
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="next">
        /// Awaitable delegate for the next action in the pipeline.
        /// </param>
        /// <returns>Awaitable task.</returns>
        Task HandleAsync(TRequest request, Func<Task> next, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// <para>
    ///     Decoradores para requisições do pipeline.
    ///     Estes objetos são executados pelo <see cref="ICommandQueryBus"/>.
    /// </para>
    /// <para>
    ///     O decorador adiciona um comportamento às requisições do pipeline, podendo modificar o valor de entrada 
    ///     ou executar alguma operação.
    /// </para>
    /// <para>
    ///     Após realizado o comportamento do decorador, deve ser chamado o próximo handler ou retornar um valor,
    ///     interrompendo a execução do pipeline.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Tipo de dado da requisição</typeparam>
    /// <typeparam name="TResult">Tipo do resultado.</typeparam>
    public interface IAsyncDecorator<in TRequest, TResult>
    {
        /// <summary>
        /// Pipeline decorator.
        /// Perform any additional behavior and await the <paramref name="next"/> delegate as necessary.
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="next">
        /// Awaitable delegate for the next action in the pipeline.
        /// </param>
        /// <returns>Awaitable task returning the <typeparamref name="TResult"/>.</returns>
        Task<TResult> HandleAsync(TRequest request, Func<Task<TResult>> next, CancellationToken cancellationToken = default);
    }
}
