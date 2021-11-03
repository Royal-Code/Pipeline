using Microsoft.Extensions.DependencyInjection;
using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.CommandAndQuery.Tests
{
    public class T02_HandlersTests
    {
        [Fact]
        public void T01_GenericHandler_In()
        {
            var sp = new ServiceCollection()
                .AddCommandAndQueryHandlersAsAService()
                .AddTransient(typeof(IHandler<>), typeof(GenericHandler<>))
                .AddSingleton<GenericHandlerService>()
                .BuildServiceProvider();
                    
            var bus = sp.GetRequiredService<ICommandQueryBus>();

            bus.Send(new GenericHandlerInRequestOne());
            bus.Send(new GenericHandlerInRequestTwo());

            var service = sp.GetRequiredService<GenericHandlerService>();

            Assert.Equal(2, service.TypesHandled.Count);
            Assert.Equal(nameof(GenericHandlerInRequestOne), service.TypesHandled[0]);
            Assert.Equal(nameof(GenericHandlerInRequestTwo), service.TypesHandled[1]);
        }

        [Fact]
        public void T02_GenericHandler_InOut()
        {
            var sp = new ServiceCollection()
                .AddCommandAndQueryHandlersAsAService()
                .AddTransient(typeof(IHandler<,>), typeof(GenericHandler<,>))
                .AddSingleton<GenericHandlerService>()
                .BuildServiceProvider();

            var bus = sp.GetRequiredService<ICommandQueryBus>();

            var oneResult = bus.Send(new GenericHandlerInOutRequestOne());
            Assert.Null(oneResult);

            var twoRequest = new GenericHandlerInOutRequestTwo();
            var twoResult = bus.Send(twoRequest);
            Assert.NotNull(twoResult);
            Assert.Same(twoRequest, twoResult);

            var service = sp.GetRequiredService<GenericHandlerService>();

            Assert.Equal(2, service.TypesHandled.Count);
            Assert.Equal(nameof(GenericHandlerInOutRequestOne), service.TypesHandled[0]);
            Assert.Equal(nameof(GenericHandlerInOutRequestTwo), service.TypesHandled[1]);
        }

        [Fact]
        public void T03_GenericHandlerWithDefinedResult_InOut()
        {
            var sp = new ServiceCollection()
                .AddCommandQueryBus(builder => builder.AddHandlerMethodDefined(typeof(GenericHandlerWithDefinedResult<>), "Handle"))
                .AddTransient(typeof(GenericHandlerWithDefinedResult<>))
                .AddSingleton<GenericHandlerService>()
                .BuildServiceProvider();

            var bus = sp.GetRequiredService<ICommandQueryBus>();

            var oneResult = bus.Send(new GenericHandlerInOutRequestOne());
            Assert.NotNull(oneResult);
            Assert.Equal(nameof(GenericHandlerInOutRequestOne), oneResult);

            Assert.ThrowsAny<Exception>(() =>
            {
                var twoRequest = new GenericHandlerInOutRequestTwo();
                var twoResult = bus.Send(twoRequest);
            });

            var service = sp.GetRequiredService<GenericHandlerService>();

            Assert.Single(service.TypesHandled);
            Assert.Equal(nameof(GenericHandlerInOutRequestOne), service.TypesHandled[0]);
        }

        [Fact]
        public void T04_GenericAsyncHandler_In()
        {
            var sp = new ServiceCollection()
                .AddCommandAndQueryHandlersAsAService()
                .AddTransient(typeof(IAsyncHandler<>), typeof(GenericAsyncHandler<>))
                .AddSingleton<GenericHandlerService>()
                .BuildServiceProvider();

            var bus = sp.GetRequiredService<ICommandQueryBus>();

            bus.Send(new GenericHandlerInRequestOne());
            bus.Send(new GenericHandlerInRequestTwo());

            var service = sp.GetRequiredService<GenericHandlerService>();

            Assert.Equal(2, service.TypesHandled.Count);
            Assert.Equal(nameof(GenericHandlerInRequestOne), service.TypesHandled[0]);
            Assert.Equal(nameof(GenericHandlerInRequestTwo), service.TypesHandled[1]);
        }

        [Fact]
        public void T05_GenericAsyncHandler_InOut()
        {
            var sp = new ServiceCollection()
                .AddCommandAndQueryHandlersAsAService()
                .AddTransient(typeof(IAsyncHandler<,>), typeof(GenericAsyncHandler<,>))
                .AddSingleton<GenericHandlerService>()
                .BuildServiceProvider();

            var bus = sp.GetRequiredService<ICommandQueryBus>();

            var oneResult = bus.Send(new GenericHandlerInOutRequestOne());
            Assert.Null(oneResult);

            var twoRequest = new GenericHandlerInOutRequestTwo();
            var twoResult = bus.Send(twoRequest);
            Assert.NotNull(twoResult);
            Assert.Same(twoRequest, twoResult);

            var service = sp.GetRequiredService<GenericHandlerService>();

            Assert.Equal(2, service.TypesHandled.Count);
            Assert.Equal(nameof(GenericHandlerInOutRequestOne), service.TypesHandled[0]);
            Assert.Equal(nameof(GenericHandlerInOutRequestTwo), service.TypesHandled[1]);
        }
    }

    public class GenericHandlerInRequestOne : IRequest { }

    public class GenericHandlerInRequestTwo : IRequest { }

    public class GenericHandlerInOutRequestOne : IRequest<string> { }

    public class GenericHandlerInOutRequestTwo : IRequest<GenericHandlerInOutRequestTwo> { }


    internal class GenericHandler<TIn> : IHandler<TIn>
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

    internal class GenericHandler<TIn, TOut> : IHandler<TIn, TOut>
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

    internal class GenericHandlerWithDefinedResult<TIn> : IHandler<TIn, string>
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

    internal class GenericAsyncHandler<TIn> : IAsyncHandler<TIn>
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

    internal class GenericAsyncHandler<TIn, TOut> : IAsyncHandler<TIn, TOut>
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

    internal class GenericHandlerService
    {

        public List<string> TypesHandled { get; set; } = new();
    }
}

