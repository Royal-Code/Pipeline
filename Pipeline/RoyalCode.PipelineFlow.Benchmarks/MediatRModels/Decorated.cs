using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Benchmarks.MediatRModels
{
    public class DecoratedRequest : IRequest
    {
        public DecoratedRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class DecoratedRequestHandler : AsyncRequestHandler<DecoratedRequest>
    {
        private readonly DecoratedService service;

        public DecoratedRequestHandler(DecoratedService service)
        {
            this.service = service;
        }

        protected override Task Handle(DecoratedRequest request, CancellationToken cancellationToken)
        {
            service.ReceivedValue += request.Value;
            return Task.CompletedTask;
        }
    }

    public class DecoratorHandlerOne<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : DecoratedRequest
    {
        private readonly DecoratedService service;

        public DecoratorHandlerOne(DecoratedService service)
        {
            this.service = service;
        }

        public Task<TResult> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            service.ReceivedValue += request.Value;
            return next();
        }
    }

    public class DecoratorHandlerTwo<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : DecoratedRequest
    {
        private readonly DecoratedService service;

        public DecoratorHandlerTwo(DecoratedService service)
        {
            this.service = service;
        }

        public Task<TResult> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
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
