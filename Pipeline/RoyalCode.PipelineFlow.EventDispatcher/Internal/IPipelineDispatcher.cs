using RoyalCode.EventDispatcher;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

public interface IPipelineDispatcher
{
    void Dispatch(object eventObject, DispatchStrategy strategy);

    Task DispatchAsync(object eventObject, DispatchStrategy strategy, CancellationToken cancellationToken);
}


