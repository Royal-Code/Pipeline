using MediatR;

namespace RoyalCode.PipelineFlow.Benchmarks.MediatRModels
{
    public class SingleWithResultRequest : IRequest<string>
    {
        public SingleWithResultRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class SingleWithResultHandler : IRequestHandler<SingleWithResultRequest, string>
    {
        private readonly SingleWithResultService service;

        public SingleWithResultHandler(SingleWithResultService service)
        {
            this.service = service;
        }

        public Task<string> Handle(SingleWithResultRequest request, CancellationToken cancellationToken)
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
