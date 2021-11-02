using MediatR;

namespace RoyalCode.PipelineFlow.Benchmarks.MediatRModels
{
    public class SingleRequest : IRequest
    {
        public SingleRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class SingleHandler : AsyncRequestHandler<SingleRequest>
    {
        private readonly SingleService service;

        public SingleHandler(SingleService service)
        {
            this.service = service;
        }

        protected override Task Handle(SingleRequest request, CancellationToken cancellationToken)
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
