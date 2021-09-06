using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T04_DefaultHandlersResolverTests
    {
        [Fact]
        public void T01_In_Handler()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.Handle<int>(i => backValue = i);
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<int>;
            Assert.NotNull(action);

            action!(1);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void T02_In_Handler_Async()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.HandleAsync<int>(i => { backValue = i; return Task.CompletedTask; });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var funtion = @delegate as Func<int, Task>;
            Assert.NotNull(funtion);

            funtion!(1);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void T03_In_Handler_Async_WithToken()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.HandleAsync<int>((i, t) => { backValue = i; return Task.CompletedTask; });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, CancellationToken, Task>;
            Assert.NotNull(function);

            function!(1, default);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void T04_Service_In_Handler()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.Handle<BackValueService, int>((s, i) => s.Value = i);
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<BackValueService, int>;
            Assert.NotNull(action);

            action!(backValueService, 1);

            Assert.Equal(1, backValueService.Value);
        }

        [Fact]
        public void T05_Service_In_Handler_Async()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.HandleAsync<BackValueService, int>((s, i) => { s.Value = i; return Task.CompletedTask; });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Task>;
            Assert.NotNull(function);

            function!(backValueService, 1);

            Assert.Equal(1, backValueService.Value);
        }

        [Fact]
        public void T06_Service_In_Handler_Async_WithToken()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.HandleAsync<BackValueService, int>((s, i, t) => { s.Value = i; return Task.CompletedTask; });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, CancellationToken, Task>;
            Assert.NotNull(function);

            function!(backValueService, 1, default);

            Assert.Equal(1, backValueService.Value);
        }

        [Fact]
        public void T07_In_Out_Handler()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.Handle<int, string>(i => { backValue = i; return i.ToString(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<int, string>;
            Assert.NotNull(func);

            var resultValue = func!(1);

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void T08_In_Out_Handler_Async()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.HandleAsync<int, string>(i => { backValue = i; return Task.FromResult(i.ToString()); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<int, Task<string>>;
            Assert.NotNull(func);

            var resultValue = func!(1).Result;

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void T09_In_Out_Handler_Async_WithToken()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.HandleAsync<int, string>((i, t) => { backValue = i; return Task.FromResult(i.ToString()); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<int, CancellationToken, Task<string>>;
            Assert.NotNull(func);

            var resultValue = func!(1, default).Result;

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void T10_Service_In_Out_Handler()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.Handle<BackValueService, int, string>((s, i) => { backValueService.Value = i; return i.ToString(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<BackValueService, int, string>;
            Assert.NotNull(func);

            var resultValue = func!(backValueService, 1);

            Assert.Equal(1, backValueService.Value);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void T11_Service_In_Out_Handler_Async()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.HandleAsync<BackValueService, int, string>((s, i) => { backValueService.Value = i; return Task.FromResult(i.ToString()); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<BackValueService, int, Task<string>>;
            Assert.NotNull(func);

            var resultValue = func!(backValueService, 1).Result;

            Assert.Equal(1, backValueService.Value);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void T12_Service_In_Out_Handler_Async_WithToken()
        {
            BackValueService backValueService = new();

            var resolver = DefaultHandlersResolver.HandleAsync<BackValueService, int, string>((s, i, t) => { backValueService.Value = i; return Task.FromResult(i.ToString()); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<BackValueService, int, CancellationToken, Task<string>>;
            Assert.NotNull(func);

            var resultValue = func!(backValueService, 1, default).Result;

            Assert.Equal(1, backValueService.Value);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void T13_In_Method_Handler()
        {
            var type = typeof(GenericMethodHandlerService<>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericMethodHandlerService<int>, int>;
            Assert.NotNull(action);

            int backValue = 0;
            var service = new GenericMethodHandlerService<int>(i => backValue = i);
            action!(service, 1);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void T14_In_Out_Method_Handler()
        {
            var type = typeof(GenericMethodHandlerService<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<GenericMethodHandlerService<int, string>, int, string>;
            Assert.NotNull(func);

            int backValue = 0;
            var service = new GenericMethodHandlerService<int, string>(i => { backValue = i; return i.ToString(); });
            var resultValue = func!(service, 1);

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void T15_In_Method_Handler_Async()
        {
            var type = typeof(GenericMethodHandlerAsyncService<>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodHandlerAsyncService<int>, int, Task>;
            Assert.NotNull(function);

            int backValue = 0;
            var service = new GenericMethodHandlerAsyncService<int>(i => backValue = i);
            function!(service, 1);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void T16_In_Out_Method_Handler_Async()
        {
            var type = typeof(GenericMethodHandlerAsyncService<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<GenericMethodHandlerAsyncService<int, string>, int, Task<string>>;
            Assert.NotNull(func);

            int backValue = 0;
            var service = new GenericMethodHandlerAsyncService<int, string>(i => { backValue = i; return i.ToString(); });
            var resultValue = func!(service, 1).Result;

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }

        [Fact]
        public void T17_In_Method_Handler_Async_WithToken()
        {
            var type = typeof(GenericMethodHandlerAsyncWithTokenService<>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodHandlerAsyncWithTokenService<int>, int, CancellationToken, Task>;
            Assert.NotNull(function);

            int backValue = 0;
            var service = new GenericMethodHandlerAsyncWithTokenService<int>(i => backValue = i);
            function!(service, 1, default);

            Assert.Equal(1, backValue);
        }

        [Fact]
        public void T18_In_Out_Method_Handler_Async_WithToken()
        {
            var type = typeof(GenericMethodHandlerAsyncWithTokenService<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodHandlerResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var func = @delegate as Func<GenericMethodHandlerAsyncWithTokenService<int, string>, int, CancellationToken, Task<string>>;
            Assert.NotNull(func);

            int backValue = 0;
            var service = new GenericMethodHandlerAsyncWithTokenService<int, string>(i => { backValue = i; return i.ToString(); });
            var resultValue = func!(service, 1, default).Result;

            Assert.Equal(1, backValue);
            Assert.Equal("1", resultValue);
        }

        private class BackValueService
        {
            public int Value { get; set; }

            public BackValueService(int value = 0)
            {
                Value = value;
            }
        }

        private class GenericMethodHandlerService<TIn>
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

        private class GenericMethodHandlerService<TIn, TOut>
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

        private class GenericMethodHandlerAsyncService<TIn>
        {
            private readonly Action<TIn> testCallback;

            public GenericMethodHandlerAsyncService(Action<TIn> testCallback)
            {
                this.testCallback = testCallback ?? throw new ArgumentNullException(nameof(testCallback));
            }

            public Task Handle(TIn input)
            {
                testCallback(input);
                return Task.CompletedTask;
            }
        }

        private class GenericMethodHandlerAsyncService<TIn, TOut>
        {
            private readonly Func<TIn, TOut> testCallback;

            public GenericMethodHandlerAsyncService(Func<TIn, TOut> testCallback)
            {
                this.testCallback = testCallback ?? throw new ArgumentNullException(nameof(testCallback));
            }

            public Task<TOut> Handle(TIn input)
            {
                return Task.FromResult(testCallback(input));
            }
        }

        private class GenericMethodHandlerAsyncWithTokenService<TIn>
        {
            private readonly Action<TIn> testCallback;

            public GenericMethodHandlerAsyncWithTokenService(Action<TIn> testCallback)
            {
                this.testCallback = testCallback ?? throw new ArgumentNullException(nameof(testCallback));
            }

            public Task Handle(TIn input, CancellationToken token)
            {
                testCallback(input);
                return Task.CompletedTask;
            }
        }

        private class GenericMethodHandlerAsyncWithTokenService<TIn, TOut>
        {
            private readonly Func<TIn, TOut> testCallback;

            public GenericMethodHandlerAsyncWithTokenService(Func<TIn, TOut> testCallback)
            {
                this.testCallback = testCallback ?? throw new ArgumentNullException(nameof(testCallback));
            }

            public Task<TOut> Handle(TIn input, CancellationToken token)
            {
                return Task.FromResult(testCallback(input));
            }
        }
    }
}
