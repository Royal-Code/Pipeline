using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.Core.Cqs
{
    /// <summary>
    /// <para>
    ///     Componente de barreamento de comandos e consultas (Command and Queries).
    ///     Comandos e consultas no sentido do CQS.
    /// </para>
    /// <para>
    ///     As consultas e comandos enviados pelo barreamento (Bus) são tratados como requests (<see cref="IRequest"/>).
    /// </para>
    /// <para>
    ///     O barreamento utiliza o componente de pipeline (<see cref="IPipeline{TRequest}"/>) para o envio das
    ///     requisições aos manipuladores (<see cref="IHandler{TRequest}"/> e outros).
    /// </para>
    /// </summary>
    public interface ICommandQueryBus
    {
        /// <summary>
        /// Envia um requisição através do barreamento para ser processada.
        /// </summary>
        /// <param name="request">Requisição de comando.</param>
        void Send(IRequest request);

        /// <summary>
        /// Envia um requisição através do barreamento para ser processada assincronamente.
        /// </summary>
        /// <param name="request">Requisição de comando.</param>
        /// <param name="token">Token para cancelamento.</param>
        /// <returns>Task</returns>
        Task SendAsync(IRequest request, CancellationToken token = default);

        /// <summary>
        /// Envia um requisição através do barreamento para ser processada e produz um resultado.
        /// </summary>
        /// <typeparam name="TResult">Tipo de dado do resultado.</typeparam>
        /// <param name="request">Requisição de comando ou consulta.</param>
        /// <returns>Resultado da requisição, comando ou consulta.</returns>
        TResult Send<TResult>(IRequest<TResult> request);

        /// <summary>
        /// Envia um requisição através do barreamento para ser processada assincronamente e produz um resultado.
        /// </summary>
        /// <typeparam name="TResult">Tipo de dado do resultado.</typeparam>
        /// <param name="request">Requisição de comando ou consulta.</param>
        /// <param name="token">Token para cancelamento.</param>
        /// <returns>Task com o resultado.</returns>
        Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token = default);
    }
}
