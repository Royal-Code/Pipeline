using RoyalCode.CommandAndQuery;

namespace RoyalCode.PipelineFlow.Benchmarks.CommandAndQueryModels
{
    public class SingleRequest : IRequest
    {
        public SingleRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class SingleHandler : IAsyncHandler<SingleRequest>
    {
        private readonly SingleService service;

        public SingleHandler(SingleService service)
        {
            this.service = service;
        }

        public Task HandleAsync(SingleRequest request, CancellationToken token = default)
        {
            service.ReceivedValue = request.Value;
            return Task.CompletedTask;
        }
    }

    public class SingleService
    {
        public int ReceivedValue { get; set; }
    }
}
