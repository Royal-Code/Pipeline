using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.SmartPipelines;

/// <summary>
/// Interface that encapsulates the pipeline,
/// where the abstract input type will be converted to the request type and the real pipeline executed.
/// </summary>
public interface IPipelineCaller
{
    /// <summary>
    /// Send the request input to be processed through the pipeline.
    /// </summary>
    /// <param name="input">The request input.</param>
    void Send(object input);

    /// <summary>
    /// Send the request input to be processed through the pipeline.
    /// </summary>
    /// <param name="input">The request input.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>Task for async processing.</returns>
    Task SendAsync(object input, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface that encapsulates the pipeline,
/// where the abstract input type will be converted to the request type and the real pipeline executed.
/// </summary>
public interface IPipelineCaller<TOut>
{
    /// <summary>
    /// Send the request input to be processed through the pipeline.
    /// </summary>
    /// <param name="input">The request input.</param>
    /// <returns>The processing result.</returns>
    TOut Send(object input);

    /// <summary>
    /// Send the request input to be processed through the pipeline.
    /// </summary>
    /// <param name="input">The request input.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>Task for async processing with the processing result.</returns>
    Task<TOut> SendAsync(object input, CancellationToken cancellationToken = default);
}
