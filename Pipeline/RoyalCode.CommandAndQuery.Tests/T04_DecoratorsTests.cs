using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.CommandAndQuery.Tests
{
    public class T04_DecoratorsTests
    {
        [Fact]
        public void T01_SimpleDecorator_In()
        {
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<SimpleDecoratorInRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new SimpleDecoratorInRequest(1);
            bus.Send(request);
            Assert.Equal(2, SimpleDecoratorInHandler.Value);

            SimpleDecoratorInHandler.Value = 0;
            bus.SendAsync(request).GetAwaiter().GetResult();
            Assert.Equal(3, SimpleDecoratorInHandler.Value);
        }

        [Fact]
        public void T02_SimpleDecorator_InOut()
        {
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<SimpleDecoratorInOutRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new SimpleDecoratorInOutRequest(1);
            var result = bus.Send(request);
            Assert.Equal(2, SimpleDecoratorInOutHandler.Value);
            Assert.Equal("2", result);

            SimpleDecoratorInOutHandler.Value = 0;
            result = bus.SendAsync(request).GetAwaiter().GetResult();
            Assert.Equal(3, SimpleDecoratorInOutHandler.Value);
            Assert.Equal("3", result);
        }

    }

    #region SimpleDecorator_In

    public class SimpleDecoratorInRequest : IRequest
    {
        public int Value { get; private set; }

        public SimpleDecoratorInRequest(int value)
        {
            Value = value;
        }

        public void Increment() => Value++;
    }

    public class SimpleDecoratorInDecorator : IDecorator<SimpleDecoratorInRequest>
    {
        public void Handle(SimpleDecoratorInRequest request, Action next)
        {
            request.Increment();
            next();
        }
    }

    public class SimpleDecoratorInHandler : IHandler<SimpleDecoratorInRequest>
    {
        public static int Value;

        public void Handle(SimpleDecoratorInRequest request)
        {
            Value = request.Value;
        }
    }

    #endregion

    #region SimpleDecorator_InOut

    public class SimpleDecoratorInOutRequest : IRequest<string>
    {
        public int Value { get; private set; }

        public SimpleDecoratorInOutRequest(int value)
        {
            Value = value;
        }

        public void Increment() => Value++;
    }

    public class SimpleDecoratorInOutDecorator : IDecorator<SimpleDecoratorInOutRequest, string>
    {
        public string Handle(SimpleDecoratorInOutRequest request, Func<string> next)
        {
            request.Increment();
            return next();
        }
    }

    public class SimpleDecoratorInOutHandler : IHandler<SimpleDecoratorInOutRequest, string>
    {
        public static int Value;

        public string Handle(SimpleDecoratorInOutRequest request)
        {
            Value = request.Value;
            return Value.ToString();
        }
    }

    #endregion
}
