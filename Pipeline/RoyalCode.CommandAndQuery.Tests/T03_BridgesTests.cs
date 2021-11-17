using Microsoft.Extensions.DependencyInjection;
using System;
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

            OneBridgeInSecondHandler.Value = 0;
            bus.SendAsync(request).GetAwaiter().GetResult();
            Assert.Equal(2, OneBridgeInSecondHandler.Value);
        }

        [Fact]
        public void T02_OneBridgeHandler_InOut()
        {
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<OneBridgeInOutFirstRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new OneBridgeInOutFirstRequest(1);
            var result = bus.Send(request);
            Assert.Equal(2, OneBridgeInOutSecondHandler.Value);
            Assert.Equal("2", result);

            OneBridgeInOutSecondHandler.Value = 0;
            result = bus.SendAsync(request).GetAwaiter().GetResult();
            Assert.Equal(2, OneBridgeInOutSecondHandler.Value);
            Assert.Equal("2", result);
        }

        [Fact]
        public void T03_OneBridgeHandler_InOutNext()
        {
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<OneBridgeInOutNextFirstRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new OneBridgeInOutNextFirstRequest(1);
            var result = bus.Send(request);
            Assert.Equal(3, OneBridgeInOutNextSecondHandler.Value);
            Assert.Equal("3", result);

            OneBridgeInOutNextSecondHandler.Value = 0;
            result = bus.SendAsync(request).GetAwaiter().GetResult();
            Assert.Equal(3, OneBridgeInOutNextSecondHandler.Value);
            Assert.Equal("3", result);
        }

        [Fact]
        public void T04_TwoBridgeHandler_InOutNext()
        {
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<TwoBridgeInOutNextFirstRequest>();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new TwoBridgeInOutNextFirstRequest(1);
            var result = bus.Send(request);
            Assert.Equal(4, TwoBridgeInOutNextSecondHandler.Value);
            Assert.Equal("4", result);

            TwoBridgeInOutNextSecondHandler.Value = 0;
            result = bus.SendAsync(request).GetAwaiter().GetResult();
            Assert.Equal(4, TwoBridgeInOutNextSecondHandler.Value);
            Assert.Equal("4", result);
        }
    }

    #region OneBridgeIn

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

    #endregion

    #region OneBridgeInOut

    public class OneBridgeInOutFirstRequest : IRequest<string>
    {
        public OneBridgeInOutFirstRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class OneBridgeInOutSecondRequest : IRequest<string>
    {
        public OneBridgeInOutSecondRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class OneBridgeInOutFirstBridgeHandler : IBridge<OneBridgeInOutFirstRequest, string, OneBridgeInOutSecondRequest>
    {
        public string Next(OneBridgeInOutFirstRequest request, Func<OneBridgeInOutSecondRequest, string> next)
        {
            var nextRequest = new OneBridgeInOutSecondRequest(request.Value + 1);
            return next(nextRequest);
        }
    }

    public class OneBridgeInOutSecondHandler : IHandler<OneBridgeInOutSecondRequest, string>
    {
        public static int Value = 0;

        public string Handle(OneBridgeInOutSecondRequest request)
        {
            Value = request.Value;
            return Value.ToString();
        }
    }

    #endregion

    #region OneBridgeInOutNext

    public class OneBridgeInOutNextFirstRequest : IRequest<string>
    {
        public OneBridgeInOutNextFirstRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class OneBridgeInOutNextSecondRequest : IRequest<Tuple<string, int>>
    {
        public OneBridgeInOutNextSecondRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class OneBridgeInOutNextFirstBridgeHandler : IBridge<OneBridgeInOutNextFirstRequest, string, OneBridgeInOutNextSecondRequest, Tuple<string, int>>
    {
        public string Next(OneBridgeInOutNextFirstRequest request, Func<OneBridgeInOutNextSecondRequest, Tuple<string, int>> next)
        {
            var nextRequest = new OneBridgeInOutNextSecondRequest(request.Value + 1);
            var result = next(nextRequest);
            return result.Item1;
        }
    }

    public class OneBridgeInOutNextSecondHandler : IHandler<OneBridgeInOutNextSecondRequest, Tuple<string, int>>
    {
        public static int Value = 0;

        public Tuple<string, int> Handle(OneBridgeInOutNextSecondRequest request)
        {
            Value = request.Value + 1;
            return new Tuple<string, int>(Value.ToString(), Value);
        }
    }

    #endregion

    #region TwoBridgeInOutNext

    public class TwoBridgeInOutNextFirstRequest : IRequest<string>
    {
        public TwoBridgeInOutNextFirstRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class TwoBridgeInOutNextSecondRequest : IRequest<Tuple<string, int>>
    {
        public TwoBridgeInOutNextSecondRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class TwoBridgeInOutNextThirdRequest : IRequest<int>
    {
        public TwoBridgeInOutNextThirdRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class TwoBridgeInOutNextFirstBridgeHandler
        : IBridge<TwoBridgeInOutNextFirstRequest, string, TwoBridgeInOutNextSecondRequest, Tuple<string, int>>
    {
        public string Next(TwoBridgeInOutNextFirstRequest request, Func<TwoBridgeInOutNextSecondRequest, Tuple<string, int>> next)
        {
            var nextRequest = new TwoBridgeInOutNextSecondRequest(request.Value + 1);
            var result = next(nextRequest);
            return result.Item1;
        }
    }

    public class TwoBridgeInOutNextSecondBridgeHandler
        : IBridge<TwoBridgeInOutNextSecondRequest, Tuple<string, int>, TwoBridgeInOutNextThirdRequest, int>
    {
        public Tuple<string, int> Next(TwoBridgeInOutNextSecondRequest request, Func<TwoBridgeInOutNextThirdRequest, int> next)
        {
            var nextRequest = new TwoBridgeInOutNextThirdRequest(request.Value + 1);
            var result = next(nextRequest);
            return new Tuple<string, int>(result.ToString(), result);
        }
    }

    public class TwoBridgeInOutNextSecondHandler : IHandler<TwoBridgeInOutNextThirdRequest, int>
    {
        public static int Value = 0;

        public int Handle(TwoBridgeInOutNextThirdRequest request)
        {
            Value = request.Value + 1;
            return Value;
        }
    }

    #endregion
}
