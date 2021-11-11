using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.CommandAndQuery.Tests
{
    [Collection("Sequential")]
    public class T03_BridgesTests
    {

        [Fact]
        public void T01_OneBridgeHandler_In()
        {
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<OneBridgeInFirstRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new OneBridgeInFirstRequest(1);

            bus.Send(request);

            Assert.Equal(2, OneBridgeInSecondHandler.Value);
        }
    }

    public class OneBridgeInFirstRequest : IRequest
    {
        public OneBridgeInFirstRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class OneBridgeInSecondRequest : IRequest
    {
        public OneBridgeInSecondRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class OneBridgeInFirstBridgeHandler : IBridge<OneBridgeInFirstRequest, OneBridgeInSecondRequest>
    {
        public void Next(OneBridgeInFirstRequest request, Action<OneBridgeInSecondRequest> next)
        {
            var nextRequest = new OneBridgeInSecondRequest(request.Value + 1);
            next(nextRequest);
        }
    }

    public class OneBridgeInSecondHandler : IHandler<OneBridgeInSecondRequest>
    {
        public static int Value = 0;

        public void Handle(OneBridgeInSecondRequest request)
        {
            Value = request.Value;
        }
    }
}
