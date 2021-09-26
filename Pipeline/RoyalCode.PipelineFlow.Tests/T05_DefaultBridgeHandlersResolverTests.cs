using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T05_DefaultBridgeHandlersResolverTests
    {
        [Fact]
        public void T01_In_Bridge()
        {
            string? backValue = null;

            var resolver = DefaultBridgeResolver.BridgeHandler<int, string>((i, next) => next(i.ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<int, Action<string>>;
            Assert.NotNull(action);

            action!(1, v => backValue = v);

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void T02_In_Bridge_Async()
        {
            string? backValue = null;

            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<int, string>((i, next) => next(i.ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, Task>, Task>;
            Assert.NotNull(function);

            function!(1, v => { backValue = v; return Task.CompletedTask; });

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void T03_In_Bridge_Async_WithToken()
        {
            string? backValue = null;

            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<int, string>((i, next, token) => next(i.ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, Task>, CancellationToken, Task>;
            Assert.NotNull(function);

            function!(1, v => { backValue = v; return Task.CompletedTask; }, default);

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void T04_Service_In_Bridge()
        {
            BackValueService backValue = new();

            var resolver = DefaultBridgeResolver.BridgeHandler<BackValueService, int, string>((s, i, next) => next((i + s.InputValue).ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<BackValueService, int, Action<string>>;
            Assert.NotNull(action);

            action!(backValue, 1, v => backValue.ResultValue = v);

            Assert.Equal("2", backValue.ResultValue);
        }

        [Fact]
        public void T05_Service_In_Bridge_Async()
        {
            BackValueService backValue = new();

            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<BackValueService, int, string>((s, i, next) => next((i + s.InputValue).ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<string, Task>, Task>;
            Assert.NotNull(function);

            function!(backValue, 1, v => { backValue.ResultValue = v; return Task.CompletedTask; });

            Assert.Equal("2", backValue.ResultValue);
        }

        [Fact]
        public void T06_Service_In_Bridge_Async_WithToken()
        {
            BackValueService backValue = new();

            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<BackValueService, int, string>((s, i, next, token) => next((i + s.InputValue).ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<string, Task>, CancellationToken, Task>;
            Assert.NotNull(function);

            function!(backValue, 1, v => { backValue.ResultValue = v; return Task.CompletedTask; }, default);

            Assert.Equal("2", backValue.ResultValue);
        }

        [Fact]
        public void T07_In_Out_Bridge()
        {
            var resolver = DefaultBridgeResolver.BridgeHandler<int, OutputValue, string>((i, next) => { return next(i.ToString()); });

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, OutputValue>, OutputValue>;
            Assert.NotNull(function);

            var result = function!(1, v => new OutputValue(v));

            Assert.Equal("1", result.Value);
        }

        [Fact]
        public void T08_In_Out_Bridge_Async()
        {
            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<int, OutputValue, string>((i, next) => { return next(i.ToString()); });

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, Task<OutputValue>>, Task<OutputValue>>;
            Assert.NotNull(function);

            var task = function!(1, v => Task.FromResult(new OutputValue(v)));

            Assert.Equal("1", task.Result.Value);
        }

        [Fact]
        public void T09_In_Out_Bridge_Async_WithToken()
        {
            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<int, OutputValue, string>((i, next, token) => { return next(i.ToString()); });

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, Task<OutputValue>>, CancellationToken, Task<OutputValue>>;
            Assert.NotNull(function);

            var task = function!(1, v => Task.FromResult(new OutputValue(v)), default);

            Assert.Equal("1", task.Result.Value);
        }

        [Fact]
        public void T10_Service_In_Out_Bridge()
        {
            var resolver = DefaultBridgeResolver.BridgeHandler<BackValueService, int, OutputValue, string>((s, i, next) => { return next((i + s.InputValue).ToString()); });

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<string, OutputValue>, OutputValue>;
            Assert.NotNull(function);

            var result = function!(new(), 1, v => new OutputValue(v));

            Assert.Equal("2", result.Value);
        }

        [Fact]
        public void T11_Service_In_Out_Bridge_Async()
        {
            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<BackValueService, int, OutputValue, string>((s, i, next) => { return next((i + s.InputValue).ToString()); });

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<string, Task<OutputValue>>, Task<OutputValue>>;
            Assert.NotNull(function);

            var task = function!(new(), 1, v => Task.FromResult(new OutputValue(v)));

            Assert.Equal("2", task.Result.Value);
        }

        [Fact]
        public void T12_Service_In_Out_Bridge_Async_WithToken()
        {
            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<BackValueService, int, OutputValue, string>((s, i, next, token) => { return next((i + s.InputValue).ToString()); });

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<string, Task<OutputValue>>, CancellationToken, Task<OutputValue>>;
            Assert.NotNull(function);

            var task = function!(new(), 1, v => Task.FromResult(new OutputValue(v)), default);

            Assert.Equal("2", task.Result.Value);
        }

        [Fact]
        public void T13_In_Out_NextOut_Bridge()
        {
            var resolver = DefaultBridgeResolver.BridgeHandler<int, string, string, OutputValue>((i, next) => { return next(i.ToString()).Value; });

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, OutputValue>, string>;
            Assert.NotNull(function);

            var result = function!(1, v => new OutputValue(v));

            Assert.Equal("1", result);
        }

        [Fact]
        public void T14_In_Out_NextOut_Bridge_Async()
        {
            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<int, string, string, OutputValue>((i, next) => { return Task.FromResult(next(i.ToString()).Result.Value); });

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, Task<OutputValue>>, Task<string>>;
            Assert.NotNull(function);

            var task = function!(1, v => Task.FromResult(new OutputValue(v)));

            Assert.Equal("1", task.Result);
        }

        [Fact]
        public void T15_In_Out_NextOut_Bridge_Async_WithResult()
        {
            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<int, string, string, OutputValue>((i, next, token) => { return Task.FromResult(next(i.ToString()).Result.Value); });

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, Task<OutputValue>>, CancellationToken, Task<string>>;
            Assert.NotNull(function);

            var task = function!(1, v => Task.FromResult(new OutputValue(v)), default);

            Assert.Equal("1", task.Result);
        }

        [Fact]
        public void T16_Service_In_Out_NextOut_Bridge()
        {
            var resolver = DefaultBridgeResolver.BridgeHandler<BackValueService, int, string, string, OutputValue>((s, i, next) => { return next((s.InputValue + i).ToString()).Value; });

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<string, OutputValue>, string>;
            Assert.NotNull(function);

            var result = function!(new(), 1, v => new OutputValue(v));

            Assert.Equal("2", result);
        }

        [Fact]
        public void T17_Service_In_Out_NextOut_BridgeAsync()
        {
            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<BackValueService, int, string, string, OutputValue>((s, i, next) => { return Task.FromResult(next((s.InputValue + i).ToString()).Result.Value); });

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<string, Task<OutputValue>>, Task<string>>;
            Assert.NotNull(function);

            var task = function!(new(), 1, v => Task.FromResult(new OutputValue(v)));

            Assert.Equal("2", task.Result);
        }

        [Fact]
        public void T18_Service_In_Out_NextOut_BridgeAsync_WithToken()
        {
            var resolver = DefaultBridgeResolver.BridgeHandlerAsync<BackValueService, int, string, string, OutputValue>((s, i, next, token) => { return Task.FromResult(next((s.InputValue + i).ToString()).Result.Value); });

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<string, Task<OutputValue>>, CancellationToken, Task<string>>;
            Assert.NotNull(function);

            var task = function!(new(), 1, v => Task.FromResult(new OutputValue(v)), default);

            Assert.Equal("2", task.Result);
        }

        [Fact]
        public void T19_In_Method_Bridge()
        {
            var type = typeof(GenericMethodBridgeHandlerService<>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodBridgeHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericMethodBridgeHandlerService<int>, int, Action<string>>;
            Assert.NotNull(action);

            string? backValue = null;
            var service = new GenericMethodBridgeHandlerService<int>(i => i.ToString(), s => backValue = s);
            action!(service, 1, s => { });

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void T20_In_Out_Method_Bridge()
        {
            var type = typeof(GenericMethodBridgeHandlerService<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodBridgeHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodBridgeHandlerService<int, OutputValue>, int, Func<string, OutputValue>, OutputValue>;
            Assert.NotNull(function);

            string? backValue = null;
            var service = new GenericMethodBridgeHandlerService<int, OutputValue>(i => i.ToString(), s => backValue = s.Value);
            function!(service, 1, s => new OutputValue(s));

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void T21_In_Out_NextOut_Method_Bridge()
        {
            var type = typeof(GenericMethodBridgeHandlerServiceWithNextOutput<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodBridgeHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodBridgeHandlerServiceWithNextOutput<int, OutputValue>, int, Func<string, string>, OutputValue>;
            Assert.NotNull(function);

            var service = new GenericMethodBridgeHandlerServiceWithNextOutput<int, OutputValue>(i => i.ToString(), s => new OutputValue(s));
            var backValue = function!(service, 1, s => s + "1");

            Assert.Equal("11", backValue.Value);
        }

        [Fact]
        public void T22_In_Method_Bridge_Async()
        {
            var type = typeof(GenericMethodBridgeHandlerAsyncService<>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodBridgeHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var funtion = @delegate as Func<GenericMethodBridgeHandlerAsyncService<int>, int, Action<string>, Task>;
            Assert.NotNull(funtion);

            string? backValue = null;
            var service = new GenericMethodBridgeHandlerAsyncService<int>(i => i.ToString(), s => backValue = s);
            funtion!(service, 1, s => { });

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void T23_In_Out_Method_Bridge_Async()
        {
            var type = typeof(GenericMethodBridgeHandlerAsyncService<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodBridgeHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodBridgeHandlerAsyncService<int, OutputValue>, int, Func<string, Task<OutputValue>>, Task<OutputValue>>;
            Assert.NotNull(function);

            string? backValue = null;
            var service = new GenericMethodBridgeHandlerAsyncService<int, OutputValue>(i => i.ToString(), s => backValue = s.Value);
            function!(service, 1, s => Task.FromResult(new OutputValue(s)));

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void T24_In_Out_NextOut_Method_Bridge_Async()
        {
            var type = typeof(GenericMethodBridgeHandlerAsyncServiceWithNextOutput<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodBridgeHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodBridgeHandlerAsyncServiceWithNextOutput<int, OutputValue>, int, Func<string, Task<string>>, Task<OutputValue>>;
            Assert.NotNull(function);

            var service = new GenericMethodBridgeHandlerAsyncServiceWithNextOutput<int, OutputValue>(i => i.ToString(), s => new OutputValue(s));
            var backValue = function!(service, 1, s => Task.FromResult(s + "1")).Result;

            Assert.Equal("11", backValue.Value);
        }

        [Fact]
        public void T25_In_Method_Bridge_Async_WithToken()
        {
            var type = typeof(GenericMethodBridgeHandlerAsyncWithTokenService<>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodBridgeHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var funtion = @delegate as Func<GenericMethodBridgeHandlerAsyncWithTokenService<int>, int, Action<string>, CancellationToken, Task>;
            Assert.NotNull(funtion);

            string? backValue = null;
            var service = new GenericMethodBridgeHandlerAsyncWithTokenService<int>(i => i.ToString(), s => backValue = s);
            funtion!(service, 1, s => { }, default);

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void T26_In_Out_Method_Bridge_Async_WithToken()
        {
            var type = typeof(GenericMethodBridgeHandlerAsyncWithTokenService<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodBridgeHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodBridgeHandlerAsyncWithTokenService<int, OutputValue>, int, Func<string, Task<OutputValue>>, CancellationToken, Task<OutputValue>>;
            Assert.NotNull(function);

            string? backValue = null;
            var service = new GenericMethodBridgeHandlerAsyncWithTokenService<int, OutputValue>(i => i.ToString(), s => backValue = s.Value);
            function!(service, 1, s => Task.FromResult(new OutputValue(s)), default);

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void T27_In_Out_NextOut_Method_Bridge_Async_WithToken()
        {
            var type = typeof(GenericMethodBridgeHandlerAsyncWithTokenServiceWithNextOutput<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodBridgeHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(OutputValue));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(OutputValue));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodBridgeHandlerAsyncWithTokenServiceWithNextOutput<int, OutputValue>, int, Func<string, Task<string>>, CancellationToken, Task<OutputValue>>;
            Assert.NotNull(function);

            var service = new GenericMethodBridgeHandlerAsyncWithTokenServiceWithNextOutput<int, OutputValue>(i => i.ToString(), s => new OutputValue(s));
            var backValue = function!(service, 1, s => Task.FromResult(s + "1"), default).Result;

            Assert.Equal("11", backValue.Value);
        }

        private class BackValueService
        {
            public int InputValue { get; set; }

            public string? ResultValue { get; set; }

            public BackValueService(int value = 1)
            {
                InputValue = value;
            }
        }

        private class OutputValue
        {
            public string Value { get; }

            public OutputValue(string value)
            {
                Value = value;
            }
        }

        private class GenericMethodBridgeHandlerService<TIn>
        {
            private readonly Func<TIn, string> inputAdapter;
            private readonly Action<string> testCallback;

            public GenericMethodBridgeHandlerService(Func<TIn, string> inputAdapter, Action<string> testCallback)
            {
                this.inputAdapter = inputAdapter;
                this.testCallback = testCallback;
            }

            public void Handle(TIn input, Action<string> next)
            {
                var nextInput = inputAdapter(input);
                next(nextInput);
                testCallback(nextInput);
            }
        }

        private class GenericMethodBridgeHandlerService<TIn, TOut>
        {
            private readonly Func<TIn, string> inputAdapter;
            private readonly Action<TOut> testCallback;

            public GenericMethodBridgeHandlerService(Func<TIn, string> inputAdapter, Action<TOut> testCallback)
            {
                this.inputAdapter = inputAdapter;
                this.testCallback = testCallback;
            }

            public TOut Handle(TIn input, Func<string, TOut> next)
            {
                var nextInput = inputAdapter(input);
                var result = next(nextInput);
                testCallback(result);
                return result;
            }
        }

        private class GenericMethodBridgeHandlerServiceWithNextOutput<TIn, TOut>
        {
            private readonly Func<TIn, string> inputAdapter;
            private readonly Func<string, TOut> outputAdapter;

            public GenericMethodBridgeHandlerServiceWithNextOutput(Func<TIn, string> inputAdapter, Func<string, TOut> outputAdapter)
            {
                this.inputAdapter = inputAdapter;
                this.outputAdapter = outputAdapter;
            }

            public TOut Handle(TIn input, Func<string, string> next)
            {
                var nextInput = inputAdapter(input);
                var nextResult = next(nextInput);
                var finalResult = outputAdapter(nextResult);
                return finalResult;
            }
        }

        private class GenericMethodBridgeHandlerAsyncService<TIn>
        {
            private readonly Func<TIn, string> inputAdapter;
            private readonly Action<string> testCallback;

            public GenericMethodBridgeHandlerAsyncService(Func<TIn, string> inputAdapter, Action<string> testCallback)
            {
                this.inputAdapter = inputAdapter;
                this.testCallback = testCallback;
            }

            public Task Handle(TIn input, Action<string> next)
            {
                var nextInput = inputAdapter(input);
                next(nextInput);
                testCallback(nextInput);
                return Task.CompletedTask;
            }
        }

        private class GenericMethodBridgeHandlerAsyncService<TIn, TOut>
        {
            private readonly Func<TIn, string> inputAdapter;
            private readonly Action<TOut> testCallback;

            public GenericMethodBridgeHandlerAsyncService(Func<TIn, string> inputAdapter, Action<TOut> testCallback)
            {
                this.inputAdapter = inputAdapter;
                this.testCallback = testCallback;
            }

            public Task<TOut> Handle(TIn input, Func<string, Task<TOut>> next)
            {
                var nextInput = inputAdapter(input);
                var result = next(nextInput).Result;
                testCallback(result);
                return Task.FromResult(result);
            }
        }

        private class GenericMethodBridgeHandlerAsyncServiceWithNextOutput<TIn, TOut>
        {
            private readonly Func<TIn, string> inputAdapter;
            private readonly Func<string, TOut> outputAdapter;

            public GenericMethodBridgeHandlerAsyncServiceWithNextOutput(Func<TIn, string> inputAdapter, Func<string, TOut> outputAdapter)
            {
                this.inputAdapter = inputAdapter;
                this.outputAdapter = outputAdapter;
            }

            public Task<TOut> Handle(TIn input, Func<string, Task<string>> next)
            {
                var nextInput = inputAdapter(input);
                var nextResult = next(nextInput).Result;
                var finalResult = outputAdapter(nextResult);
                return Task.FromResult(finalResult);
            }
        }

        private class GenericMethodBridgeHandlerAsyncWithTokenService<TIn>
        {
            private readonly Func<TIn, string> inputAdapter;
            private readonly Action<string> testCallback;

            public GenericMethodBridgeHandlerAsyncWithTokenService(Func<TIn, string> inputAdapter, Action<string> testCallback)
            {
                this.inputAdapter = inputAdapter;
                this.testCallback = testCallback;
            }

            public Task Handle(TIn input, Action<string> next, CancellationToken cancellationToken)
            {
                var nextInput = inputAdapter(input);
                next(nextInput);
                testCallback(nextInput);
                return Task.CompletedTask;
            }
        }

        private class GenericMethodBridgeHandlerAsyncWithTokenService<TIn, TOut>
        {
            private readonly Func<TIn, string> inputAdapter;
            private readonly Action<TOut> testCallback;

            public GenericMethodBridgeHandlerAsyncWithTokenService(Func<TIn, string> inputAdapter, Action<TOut> testCallback)
            {
                this.inputAdapter = inputAdapter;
                this.testCallback = testCallback;
            }

            public Task<TOut> Handle(TIn input, Func<string, Task<TOut>> next, CancellationToken cancellationToken)
            {
                var nextInput = inputAdapter(input);
                var result = next(nextInput).Result;
                testCallback(result);
                return Task.FromResult(result);
            }
        }

        private class GenericMethodBridgeHandlerAsyncWithTokenServiceWithNextOutput<TIn, TOut>
        {
            private readonly Func<TIn, string> inputAdapter;
            private readonly Func<string, TOut> outputAdapter;

            public GenericMethodBridgeHandlerAsyncWithTokenServiceWithNextOutput(Func<TIn, string> inputAdapter, Func<string, TOut> outputAdapter)
            {
                this.inputAdapter = inputAdapter;
                this.outputAdapter = outputAdapter;
            }

            public Task<TOut> Handle(TIn input, Func<string, Task<string>> next, CancellationToken cancellationToken)
            {
                var nextInput = inputAdapter(input);
                var nextResult = next(nextInput).Result;
                var finalResult = outputAdapter(nextResult);
                return Task.FromResult(finalResult);
            }
        }
    }
}
