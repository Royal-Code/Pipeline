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
    public class DefaultHandlersResolverTests
    {

        [Fact]
        public void _01_Action_Handler()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.Handle<int>(i => backValue = i);
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<int>;
            Assert.NotNull(action);

            action(1);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void _02_Func_Handler_Async()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.Handle<int>(i => { backValue = i; return Task.CompletedTask; });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Func<int, Task>;
            Assert.NotNull(action);

            action(1);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void _03_Func_Handler_Async_WithToken()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.Handle<int>((i, t) => { backValue = i; return Task.CompletedTask; });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Func<int, CancellationToken, Task>;
            Assert.NotNull(action);

            action(1, default);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void _04_Service_Action_Handler()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.Handle<BackValueService, int>((s, i) => s.Value = i);
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<BackValueService, int>;
            Assert.NotNull(action);

            action(backValueService, 1);

            Assert.Equal(1, backValueService.Value);
        }

        [Fact]
        public void _05_Service_Func_Handler_Async()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.Handle<BackValueService, int>((s, i) => { s.Value = i; return Task.CompletedTask; });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Func<BackValueService, int, Task>;
            Assert.NotNull(action);

            action(backValueService, 1);

            Assert.Equal(1, backValueService.Value);
        }

        [Fact]
        public void _06_Service_Func_Handler_Async_WithToken()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.Handle<BackValueService, int>((s, i, t) => { s.Value = i; return Task.CompletedTask; });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Func<BackValueService, int, CancellationToken, Task>;
            Assert.NotNull(action);

            action(backValueService, 1, default);

            Assert.Equal(1, backValueService.Value);
        }

        [Fact]
        public void _07_Func_Result_Handler()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.Handle<int, string>(i => { backValue = i; return i.ToString(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<int, string>;
            Assert.NotNull(func);

            var resultValue = func(1);

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void _08_Func_Result_Handler_Async()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.Handle<int, string>(i => { backValue = i; return Task.FromResult(i.ToString()); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<int, Task<string>>;
            Assert.NotNull(func);

            var resultValue = func(1).Result;

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void _09_Func_Result_Handler_Async_WithToken()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.Handle<int, string>((i, t) => { backValue = i; return Task.FromResult(i.ToString()); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<int, CancellationToken, Task<string>>;
            Assert.NotNull(func);

            var resultValue = func(1, default).Result;

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void _10_Service_Func_Result_Handler()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.Handle<BackValueService, int, string>((s, i) => { backValueService.Value = i; return i.ToString(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<BackValueService, int, string>;
            Assert.NotNull(func);

            var resultValue = func(backValueService, 1);

            Assert.Equal(1, backValueService.Value);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void _11_Service_Func_Result_Handler_Async()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.Handle<BackValueService, int, string>((s, i) => { backValueService.Value = i; return Task.FromResult(i.ToString()); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<BackValueService, int, Task<string>>;
            Assert.NotNull(func);

            var resultValue = func(backValueService, 1).Result;

            Assert.Equal(1, backValueService.Value);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void _12_Service_Func_Result_Handler_Async_WithToken()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.Handle<BackValueService, int, string>((s, i, t) => { backValueService.Value = i; return Task.FromResult(i.ToString()); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<BackValueService, int, CancellationToken, Task<string>>;
            Assert.NotNull(func);

            var resultValue = func(backValueService, 1, default).Result;

            Assert.Equal(1, backValueService.Value);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void _13_Method_Handler()
        {
            var type = typeof(GenericMethodHandlerService<>);
            var method = type.GetMethod("Handle");
            var resolver = new MethodHandlerResolver(method);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericMethodHandlerService<int>, int>;
            Assert.NotNull(action);

            int backValue = 0;
            var service = new GenericMethodHandlerService<int>(i => backValue = i);
            action(service, 1);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void _14_Method_Result_Handler()
        {
            var type = typeof(GenericMethodHandlerService<,>);
            var method = type.GetMethod("Handle");
            var resolver = new MethodHandlerResolver(method);

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<GenericMethodHandlerService<int, string>, int, string>;
            Assert.NotNull(func);

            int backValue = 0;
            var service = new GenericMethodHandlerService<int, string>(i => { backValue = i; return i.ToString(); });
            var resultValue = func(service, 1);

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }
    }

    public class BackValueService
    {
        public int Value { get; set; }

        public BackValueService(int value = 0)
        {
            Value = value;
        }
    }

    public class GenericMethodHandlerService<TIn>
    {
        private readonly Action<TIn> testCallback;

        public GenericMethodHandlerService(Action<TIn> testCallback)
        {
            this.testCallback = testCallback ?? throw new ArgumentNullException(nameof(testCallback));
        }

        public void Handle(TIn input)
        {
            testCallback(input);
        }
    }

    public class GenericMethodHandlerService<TIn, TOut>
    {
        private readonly Func<TIn, TOut> testCallback;

        public GenericMethodHandlerService(Func<TIn, TOut> testCallback)
        {
            this.testCallback = testCallback ?? throw new ArgumentNullException(nameof(testCallback));
        }

        public TOut Handle(TIn input)
        {
            return testCallback(input);
        }
    }
}
