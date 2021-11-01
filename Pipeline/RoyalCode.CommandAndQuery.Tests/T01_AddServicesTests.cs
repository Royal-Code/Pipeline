using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace RoyalCode.CommandAndQuery.Tests
{
    public class T01_AddServicesTests
    {
        [Fact]
        public void T01_Basic_Get_ICommandQueryBus()
        {
            var services = new ServiceCollection();

            services.AddCommandQueryBus();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);
        }

        [Fact]
        public void T02_Basic_SendRequestToHandler_ConfiguratedByFromAssemblyMethod()
        {
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<AddServicesTestRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new AddServicesTestRequest();

            bus.Send(request);
        }

        [Fact]
        public void T03_Basic_SendRequestToHandler_ConfiguratedByHandlersAsAServiceMethod()
        {
            var services = new ServiceCollection();

            services.AddCommandAndQueryHandlersAsAService();
            services.AddTransient<IHandler<AddServicesTestRequest>, AddServicesTestHandler>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new AddServicesTestRequest();

            bus.Send(request);
        }
    }

    public class AddServicesTestRequest : IRequest { }

    public class AddServicesTestHandler : IHandler<AddServicesTestRequest>
    {
        public void Handle(AddServicesTestRequest request)
        {
            Console.WriteLine("Test Handled");
        }
    }
}
