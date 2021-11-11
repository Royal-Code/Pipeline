namespace RoyalCode.CommandAndQuery.Tests.GenericsHandlers
{
    public class GenericHandlerInRequestOne : IRequest { }

    public class GenericHandlerInRequestTwo : IRequest { }

    public class GenericHandlerInOutRequestOne : IRequest<string> { }

    public class GenericHandlerInOutRequestTwo : IRequest<GenericHandlerInOutRequestTwo> { }


    public class GenericHandler<TIn> : IHandler<TIn>
        where TIn : class, IRequest
    {
        private readonly GenericHandlerService service;

        public GenericHandler(GenericHandlerService service)
        {
            this.service = service;
        }

        public void Handle(TIn request)
        {
            service.TypesHandled.Add(typeof(TIn).Name);
        }
    }

    public class GenericHandler<TIn, TOut> : IHandler<TIn, TOut>
        where TIn : class, IRequest<TOut>
    {
        private readonly GenericHandlerService service;

        public GenericHandler(GenericHandlerService service)
        {
            this.service = service;
        }

        public TOut Handle(TIn request)
        {
            service.TypesHandled.Add(typeof(TIn).Name);

            return request is TOut @out ? @out : default;
        }
    }

    public class GenericHandlerWithDefinedResult<TIn> : IHandler<TIn, string>
        where TIn : class, IRequest<string>
    {
        private readonly GenericHandlerService service;

        public GenericHandlerWithDefinedResult(GenericHandlerService service)
        {
            this.service = service;
        }

        public string Handle(TIn request)
        {
            service.TypesHandled.Add(typeof(TIn).Name);

            return typeof(TIn).Name;
        }
    }

    public class GenericAsyncHandler<TIn> : IAsyncHandler<TIn>
        where TIn : class, IRequest
    {
        private readonly GenericHandlerService service;

        public GenericAsyncHandler(GenericHandlerService service)
        {
            this.service = service;
        }

        public Task HandleAsync(TIn request, CancellationToken token = default)
        {
            service.TypesHandled.Add(typeof(TIn).Name);
            return Task.CompletedTask;
        }
    }

    public class GenericAsyncHandler<TIn, TOut> : IAsyncHandler<TIn, TOut>
        where TIn : class, IRequest<TOut>
    {
        private readonly GenericHandlerService service;

        public GenericAsyncHandler(GenericHandlerService service)
        {
            this.service = service;
        }

        public TOut Handle(TIn request)
        {
            service.TypesHandled.Add(typeof(TIn).Name);

            return request is TOut @out ? @out : default;
        }

        public Task<TOut> HandleAsync(TIn request, CancellationToken token = default)
        {
            service.TypesHandled.Add(typeof(TIn).Name);

            return Task.FromResult(request is TOut @out ? @out : default);
        }
    }

    public class GenericHandlerService
    {

        public List<string> TypesHandled { get; set; } = new();
    }
}