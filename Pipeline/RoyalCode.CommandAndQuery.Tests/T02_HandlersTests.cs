using Microsoft.Extensions.DependencyInjection;
using RoyalCode.CommandAndQuery.Tests.GenericsHandlers;
using RoyalCode.PipelineFlow.Configurations;
using System;
using Xunit;

namespace RoyalCode.CommandAndQuery.Tests
{
    [Collection("Sequential")]
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
}

