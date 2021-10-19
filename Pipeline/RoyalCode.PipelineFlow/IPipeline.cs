using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow
{
    /// <summary>
    /// <para>
    ///     A pipeline for sending requests of type <typeparamref name="TIn"/> without producing a result.
    /// </para>
    /// <para>
    ///     The pipeline work with some design patters: mediator, chain of responsibility, bridge, decorators and command.
    /// </para>
    /// <para>
    ///     The pipeline itself is a mediator, which will send a command to various handlers to be processed.
    ///     These handlers are organized as a chain, the pattern chain of responsibility.
    ///     The chain of handlers is composed of decorators and a processing handler.
    ///     The processing handler can continue the chain with another command, which can be considered the bridge pattern.
    /// </para>
    /// </summary>
    /// <typeparam name="TIn">The request input type.</typeparam>
    public interface IPipeline<in TIn> 
    {
        /// <summary>
        /// Send the request input to be processed through the pipeline.
        /// </summary>
        /// <param name="input">The request input.</param>
        void Send(TIn input);

        /// <summary>
        /// Send the request input to be processed through the pipeline.
        /// </summary>
        /// <param name="input">The request input.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>Task for async processing.</returns>
        Task SendAsync(TIn input, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// <para>
    ///     A pipeline for sending requests of type <typeparamref name="TIn"/> producing a result of type
    ///     <typeparamref name="TOut"/>.
    /// </para>
    /// <para>
    ///     The pipeline work with some design patters: mediator, chain of responsibility, bridge, decorators and command.
    /// </para>
    /// <para>
    ///     The pipeline itself is a mediator, which will send a command to various handlers to be processed.
    ///     These handlers are organized as a chain, the pattern chain of responsibility.
    ///     The chain of handlers is composed of decorators and a processing handler.
    ///     The processing handler can continue the chain with another command, which can be considered the bridge pattern.
    /// </para>
    /// </summary>
    /// <typeparam name="TIn">The request input type.</typeparam>
    /// <typeparam name="TOut">The result output type.</typeparam>
    public interface IPipeline<in TIn, TOut> 
    {
        /// <summary>
        /// Send the request input to be processed through the pipeline.
        /// </summary>
        /// <param name="input">The request input.</param>
        /// <returns>The processing result.</returns>
        TOut Send(TIn input);

        /// <summary>
        /// Send the request input to be processed through the pipeline.
        /// </summary>
        /// <param name="input">The request input.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>Task for async processing with the processing result.</returns>
        Task<TOut> SendAsync(TIn input, CancellationToken cancellationToken = default);
    }
}
