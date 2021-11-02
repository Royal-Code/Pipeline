using RoyalCode.CommandAndQuery;

namespace RoyalCode.PipelineFlow.Benchmarks.CommandAndQueryModels
{
    public class DecoratedRequest : IRequest
    {
        public DecoratedRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class DecoratedRequestHandler : IAsyncHandler<DecoratedRequest>
    {
        private readonly DecoratedService service;

        public DecoratedRequestHandler(DecoratedService service)
        {
            this.service = service;
        }

        public Task HandleAsync(DecoratedRequest request, CancellationToken token = default)
        {
            service.ReceivedValue += request.Value;
            return Task.CompletedTask;
        }
    }

    public class DecoratorHandlerOne : IAsyncDecorator<DecoratedRequest>
    {
        private readonly DecoratedService service;

        public DecoratorHandlerOne(DecoratedService service)
        {
            this.service = service;
        }

        public Task HandleAsync(DecoratedRequest request, Func<Task> next, CancellationToken cancellationToken = default)
        {
            service.ReceivedValue += request.Value;
            return next();
        }
    }

    public class DecoratorHandlerTwo : IAsyncDecorator<DecoratedRequest>
    {
        private readonly DecoratedService service;

        public DecoratorHandlerTwo(DecoratedService service)
        {
            this.service = service;
        }

        public Task HandleAsync(DecoratedRequest request, Func<Task> next, CancellationToken cancellationToken = default)
        {
            service.ReceivedValue += 1;
            return next();
        }
    }

    public class DecoratedService
    {
        public int ReceivedValue { get; set; }
    }
}
