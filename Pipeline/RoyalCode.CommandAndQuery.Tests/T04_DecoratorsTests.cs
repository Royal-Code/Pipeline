using Microsoft.Extensions.DependencyInjection;
using RoyalCode.CommandAndQuery.Tests.GenericsDecorators;
using RoyalCode.PipelineFlow;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.CommandAndQuery.Tests
{
    public class T04_DecoratorsTests
    {
        [Fact]
        public async Task T01_SimpleDecorator_In()
        {
            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<SimpleDecoratorInRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new SimpleDecoratorInRequest(1);
            bus.Send(request);
            Assert.Equal(2, SimpleDecoratorInHandler.Value);

            SimpleDecoratorInHandler.Value = 0;
            await bus.SendAsync(request);
            Assert.Equal(3, SimpleDecoratorInHandler.Value);
        }

        [Fact]
        public async Task T02_SimpleDecorator_InOut()
        {
            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
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
            result = await bus.SendAsync(request);
            Assert.Equal(3, SimpleDecoratorInOutHandler.Value);
            Assert.Equal("3", result);
        }

        [Fact]
        public async Task T03_MultiDecorator_In()
        {
            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<MultiDecoratorInRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new MultiDecoratorInRequest(1);
            bus.Send(request);
            Assert.Equal(4, MultiDecoratorInHandler.Value);

            MultiDecoratorInHandler.Value = 0;
            await bus.SendAsync(request);
            Assert.Equal(7, MultiDecoratorInHandler.Value);
        }

        [Fact]
        public async Task T04_MultiDecorator_InOut()
        {
            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<MultiDecoratorInOutRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new MultiDecoratorInOutRequest(1);
            var result = bus.Send(request);
            Assert.Equal(4, MultiDecoratorInOutHandler.Value);
            Assert.Equal("4", result);

            MultiDecoratorInOutHandler.Value = 0;
            result = await bus.SendAsync(request);
            Assert.Equal(7, MultiDecoratorInOutHandler.Value);
            Assert.Equal("7", result);
        }

        [Fact]
        public async Task T05_GenericDecorator_In()
        {
            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<GenericDecoratorIn<object>>();
            services.AddCommandsAndQueriesFromAssemblyOfType<GenericDecoratorInRequest>();
            services.AddSingleton<Action<GenericDecoratorInRequest>>(request => request.Increment());

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new GenericDecoratorInRequest(1);
            bus.Send(request);
            Assert.Equal(2, GenericDecoratorInHandler.Value);

            GenericDecoratorInHandler.Value = 0;
            await bus.SendAsync(request);
            Assert.Equal(3, GenericDecoratorInHandler.Value);
        }

        [Fact]
        public async Task T06_GenericDecorator_InOut()
        {
            string decoratorOutput = null;

            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<GenericDecoratorInOut<object, object>>();
            services.AddCommandsAndQueriesFromAssemblyOfType<GenericDecoratorInOutRequest>();
            services.AddSingleton<Action<GenericDecoratorInOutRequest>>(request => request.Increment());
            services.AddSingleton<Action<string>>(output => decoratorOutput = output);

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new GenericDecoratorInOutRequest(1);
            var result = bus.Send(request);
            Assert.Equal(2, GenericDecoratorInOutHandler.Value);
            Assert.Equal("2", result);
            Assert.Equal(result, decoratorOutput);

            decoratorOutput = null;
            GenericDecoratorInOutHandler.Value = 0;
            result = await bus.SendAsync(request);
            Assert.Equal(3, GenericDecoratorInOutHandler.Value);
            Assert.Equal("3", result);
            Assert.Equal(result, decoratorOutput);
        }

        [Fact]
        public async Task T07_CircuitBreak_In()
        {
            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<CircuitBreakDecoratorInRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new CircuitBreakDecoratorInRequest(1);
            bus.Send(request);
            Assert.Equal(0, CircuitBreakDecoratorInHandler.Value);
            Assert.Equal(2, request.Value);

            await bus.SendAsync(request);
            Assert.Equal(0, CircuitBreakDecoratorInHandler.Value);
            Assert.Equal(3, request.Value);
        }

        [Fact]
        public async Task T08_CircuitBreak_InOut()
        {
            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<CircuitBreakDecoratorInOutRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new CircuitBreakDecoratorInOutRequest(1);
            var result = bus.Send(request);
            Assert.Equal(0, CircuitBreakDecoratorInOutHandler.Value);
            Assert.Equal(2, request.Value);
            Assert.Equal("CircuitBreak", result);

            result = await bus.SendAsync(request);
            Assert.Equal(0, CircuitBreakDecoratorInOutHandler.Value);
            Assert.Equal(3, request.Value);
            Assert.Equal("CircuitBreak", result);
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

    #region MultiDecorators_In

    public class MultiDecoratorInRequest : IRequest
    {
        public int Value { get; private set; }

        public MultiDecoratorInRequest(int value)
        {
            Value = value;
        }

        public void Increment() => Value++;
    }

    public class MultiDecoratorInDecorator_1 : IDecorator<MultiDecoratorInRequest>
    {
        public void Handle(MultiDecoratorInRequest request, Action next)
        {
            request.Increment();
            next();
        }
    }

    public class MultiDecoratorInDecorator_2 : IDecorator<MultiDecoratorInRequest>
    {
        public void Handle(MultiDecoratorInRequest request, Action next)
        {
            request.Increment();
            next();
        }
    }

    public class MultiDecoratorInDecorator_3 : IDecorator<MultiDecoratorInRequest>
    {
        public void Handle(MultiDecoratorInRequest request, Action next)
        {
            request.Increment();
            next();
        }
    }

    public class MultiDecoratorInHandler : IHandler<MultiDecoratorInRequest>
    {
        public static int Value;

        public void Handle(MultiDecoratorInRequest request)
        {
            Value = request.Value;
        }
    }

    #endregion

    #region MultiDecorators_InOut

    public class MultiDecoratorInOutRequest : IRequest<string>
    {
        public int Value { get; private set; }

        public MultiDecoratorInOutRequest(int value)
        {
            Value = value;
        }

        public void Increment() => Value++;
    }

    public class MultiDecoratorInOutDecorator_1 : IDecorator<MultiDecoratorInOutRequest, string>
    {
        public string Handle(MultiDecoratorInOutRequest request, Func<string> next)
        {
            request.Increment();
            return next();
        }
    }

    public class MultiDecoratorInOutDecorator_2 : IDecorator<MultiDecoratorInOutRequest, string>
    {
        public string Handle(MultiDecoratorInOutRequest request, Func<string> next)
        {
            request.Increment();
            return next();
        }
    }

    public class MultiDecoratorInOutDecorator_3 : IDecorator<MultiDecoratorInOutRequest, string>
    {
        public string Handle(MultiDecoratorInOutRequest request, Func<string> next)
        {
            request.Increment();
            return next();
        }
    }

    public class MultiDecoratorInOutHandler : IHandler<MultiDecoratorInOutRequest, string>
    {
        public static int Value;

        public string Handle(MultiDecoratorInOutRequest request)
        {
            Value = request.Value;
            return Value.ToString();
        }
    }

    #endregion

    #region GenericDecorator_In

    public class GenericDecoratorInRequest : IRequest
    {
        public int Value { get; private set; }

        public GenericDecoratorInRequest(int value)
        {
            Value = value;
        }

        public void Increment() => Value++;
    }

    public class GenericDecoratorInHandler : IHandler<GenericDecoratorInRequest>
    {
        public static int Value;

        public void Handle(GenericDecoratorInRequest request)
        {
            Value = request.Value;
        }
    }

    #endregion

    #region GenericDecorator_InOut

    public class GenericDecoratorInOutRequest : IRequest<string>
    {
        public int Value { get; private set; }

        public GenericDecoratorInOutRequest(int value)
        {
            Value = value;
        }

        public void Increment() => Value++;
    }

    public class GenericDecoratorInOutHandler : IHandler<GenericDecoratorInOutRequest, string>
    {
        public static int Value;

        public string Handle(GenericDecoratorInOutRequest request)
        {
            Value = request.Value;
            return Value.ToString();
        }
    }

    #endregion

    #region CircuitBreak_In

    public class CircuitBreakDecoratorInRequest : IRequest
    {
        public int Value { get; private set; }

        public CircuitBreakDecoratorInRequest(int value)
        {
            Value = value;
        }

        public void Increment() => Value++;
    }

    public class CircuitBreakDecoratorInDecorator : IDecorator<CircuitBreakDecoratorInRequest>
    {
        public void Handle(CircuitBreakDecoratorInRequest request, Action next)
        {
            request.Increment();
        }
    }

    public class CircuitBreakDecoratorInHandler : IHandler<CircuitBreakDecoratorInRequest>
    {
        public static int Value;

        public void Handle(CircuitBreakDecoratorInRequest request)
        {
            request.Increment();
            Value = request.Value;
        }
    }

    #endregion

    #region CircuitBreak_InOut

    public class CircuitBreakDecoratorInOutRequest : IRequest<string>
    {
        public int Value { get; private set; }

        public CircuitBreakDecoratorInOutRequest(int value)
        {
            Value = value;
        }

        public void Increment() => Value++;
    }

    public class CircuitBreakDecoratorInOutDecorator : IDecorator<CircuitBreakDecoratorInOutRequest, string>
    {
        public string Handle(CircuitBreakDecoratorInOutRequest request, Func<string> next)
        {
            request.Increment();
            return "CircuitBreak";
        }
    }

    public class CircuitBreakDecoratorInOutHandler : IHandler<CircuitBreakDecoratorInOutRequest, string>
    {
        public static int Value;

        public string Handle(CircuitBreakDecoratorInOutRequest request)
        {
            request.Increment();
            Value = request.Value;
            return Value.ToString();
        }
    }

    #endregion
}
