using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow
{
    public interface IPipeline<in TIn> 
    {
        void Send(TIn input);

        Task SendAsync(TIn input, CancellationToken cancellationToken = default);
    }

    public interface IPipeline<in TIn, TOut> 
    {
        TOut Send(TIn input);

        Task<TOut> SendAsync(TIn input, CancellationToken cancellationToken = default);
    }
}
