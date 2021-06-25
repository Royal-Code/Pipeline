using System;

namespace RoyalCode.Core.Cqs
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
    ///     Após realizado o comportamento do decorador, deve ser chamado o próximo handler ou retornar um valor,
    ///     interrompendo a execução do pipeline.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Tipo de dado da requisição</typeparam>
    /// <typeparam name="TResult">Tipo do resultado.</typeparam>
    public interface IDecorator<in TRequest, TResult>
    {
        /// <summary>
        /// Pipeline decorator.
        /// Perform any additional behavior and call <paramref name="next"/> delegate as necessary.
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <param name="next">
        /// Delegate for the next action in the pipeline.
        /// </param>
        /// <returns>The <typeparamref name="TResult"/>.</returns>
        TResult Handle(TRequest request, Func<TResult> next);
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
    ///     Após realizado o comportamento do decorador, deve ser chamado o próximo handler, caso contrário a 
    ///     execução será interrompida.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Tipo de dado da requisição.</typeparam>
    public interface IDecorator<in TRequest>
    {
        /// <summary>
        /// Pipeline decorator.
        /// Perform any additional behavior and call <paramref name="next"/> delegate as necessary.
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <param name="next">
        /// Delegate for the next action in the pipeline.
        /// </param>
        /// <returns>The <typeparamref name="TRequest"/>.</returns>
        void Handle(TRequest request, Action next);
    }
}
