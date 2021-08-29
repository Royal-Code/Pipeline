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
    public class DefaultBridgeHandlersResolverTests
    {
        [Fact]
        public void _01_Action_Bridge()
        {
            string? backValue = null;

            var resolver = DefaultHandlersResolver.BridgeHandler<int, string>((i, next) => next(i.ToString()));

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
        public void _02_Func_Bridge_Async()
        {
            string? backValue = null;

            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<int, string>((i, next) => next(i.ToString()));

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
        public void _03_Func_Bridge_Async_WithToken()
        {
            string? backValue = null;

            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<int, string>((i, next, token) => next(i.ToString()));

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
        public void _04_Service_Action_Bridge()
        {
            BackValueService backValue = new();

            var resolver = DefaultHandlersResolver.BridgeHandler<BackValueService, int, string>((s, i, next) => next((i + s.InputValue).ToString()));

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
        public void _05_Service_Bridge_Async()
        {
            BackValueService backValue = new();

            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<BackValueService, int, string>((s, i, next) => next((i + s.InputValue).ToString()));

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
        public void _05_Service_Bridge_Async_WithToken()
        {
            BackValueService backValue = new();

            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<BackValueService, int, string>((s, i, next, token) => next((i + s.InputValue).ToString()));

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
        public void _07_Func_Result_Bridge()
        {
            var resolver = DefaultHandlersResolver.BridgeHandler<int, OutputValue, string>((i, next) => { return next(i.ToString()); });

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
        public void _08_Func_Result_Bridge_Async()
        {
            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<int, OutputValue, string>((i, next) => { return next(i.ToString()); });

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
        public void _09_Func_Result_Bridge_Async_WithToken()
        {
            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<int, OutputValue, string>((i, next, token) => { return next(i.ToString()); });

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
        public void _10_Service_Result_Bridge()
        {
            var resolver = DefaultHandlersResolver.BridgeHandler<BackValueService, int, OutputValue, string>((s, i, next) => { return next((i + s.InputValue).ToString()); });

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
        public void _11_Service_Result_Bridge_Async()
        {
            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<BackValueService, int, OutputValue, string>((s, i, next) => { return next((i + s.InputValue).ToString()); });

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
        public void _12_Service_Result_Bridge_Async_WithToken()
        {
            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<BackValueService, int, OutputValue, string>((s, i, next, token) => { return next((i + s.InputValue).ToString()); });

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
        public void _13_Func_Result_NextResult_Bridge()
        {
            var resolver = DefaultHandlersResolver.BridgeHandler<int, string, string, OutputValue>((i, next) => { return next(i.ToString()).Value; });

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
        public void _14_Func_Result_NextResult_Bridge_Async()
        {
            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<int, string, string, OutputValue>((i, next) => { return Task.FromResult(next(i.ToString()).Result.Value); });

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
        public void _15_Func_Result_NextResult_Bridge_Async_WithResult()
        {
            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<int, string, string, OutputValue>((i, next, token) => { return Task.FromResult(next(i.ToString()).Result.Value); });

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
        public void _16_Service_Result_NextResult_Bridge()
        {
            var resolver = DefaultHandlersResolver.BridgeHandler<BackValueService, int, string, string, OutputValue>((s, i, next) => { return next((s.InputValue + i).ToString()).Value; });

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
        public void _17_Service_Result_NextResult_BridgeAsync()
        {
            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<BackValueService, int, string, string, OutputValue>((s, i, next) => { return Task.FromResult(next((s.InputValue + i).ToString()).Result.Value); });

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
        public void _18_Service_Result_NextResult_BridgeAsync_WithToken()
        {
            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<BackValueService, int, string, string, OutputValue>((s, i, next, token) => { return Task.FromResult(next((s.InputValue + i).ToString()).Result.Value); });

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
        public void _19_Method_Bridge()
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
        public void _20_Method_Result_Bridge()
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
    }
}
