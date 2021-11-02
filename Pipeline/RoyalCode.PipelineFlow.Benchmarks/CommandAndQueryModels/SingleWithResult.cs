using RoyalCode.CommandAndQuery;

namespace RoyalCode.PipelineFlow.Benchmarks.CommandAndQueryModels
{
    public class SingleWithResultRequest : IRequest<string>
    {
        public SingleWithResultRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class SingleWithResultHandler : IAsyncHandler<SingleWithResultRequest, string>
    {
        private readonly SingleWithResultService service;

        public SingleWithResultHandler(SingleWithResultService service)
        {
            this.service = service;
        }

        public Task<string> HandleAsync(SingleWithResultRequest request, CancellationToken token = default)
        {
            service.ReceivedValue = request.Value;
            return Task.FromResult(request.Value.ToString());
        }
    }

    public class SingleWithResultService
    {
        public int ReceivedValue { get; set; }
    }
}
