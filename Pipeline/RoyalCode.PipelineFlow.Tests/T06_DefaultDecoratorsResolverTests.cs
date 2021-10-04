using RoyalCode.PipelineFlow.Resolvers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T06_DefaultDecoratorsResolverTests
    {
        [Fact]
        public void T01_In_Decorator()
        {
            int backValue1 = 0, backValue2 = 0;

            var resolver = DefaultDecoratorsResolver.Decorate<int>((i, next) => { backValue1 = i; next(); });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<int, Action>;
            Assert.NotNull(action);

            action!(1, () => backValue2 = 2);

            Assert.Equal(1, backValue1);
            Assert.Equal(2, backValue2);
        }

        [Fact]
        public void T02_In_Decorator_Async()
        {
            int backValue1 = 0, backValue2 = 0;

            var resolver = DefaultDecoratorsResolver.DecorateAsync<int>((i, next) => { backValue1 = i; return next(); });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var funtion = @delegate as Func<int, Func<Task>, Task>;
            Assert.NotNull(funtion);

            funtion!(1, () => { backValue2 = 2; return Task.CompletedTask; });

            Assert.Equal(1, backValue1);
            Assert.Equal(2, backValue2);
        }

        [Fact]
        public void T03_In_Decorator_Async_WithToken()
        {
            int backValue1 = 0, backValue2 = 0;

            var resolver = DefaultDecoratorsResolver.DecorateAsync<int>((i, next, token) => { backValue1 = i; return next(); });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var funtion = @delegate as Func<int, Func<Task>, CancellationToken, Task>;
            Assert.NotNull(funtion);

            funtion!(1, () => { backValue2 = 2; return Task.CompletedTask; }, default);

            Assert.Equal(1, backValue1);
            Assert.Equal(2, backValue2);
        }

        [Fact]
        public void T04_Service_In_Decorator()
        {
            BackValueService backValue1 = new(), backValue2 = new();

            var resolver = DefaultDecoratorsResolver.Decorate<BackValueService, int>((s, i, next) => { s.Value = i; next(); });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<BackValueService, int, Action>;
            Assert.NotNull(action);

            action!(backValue1, 1, () => backValue2.Value = 2);

            Assert.Equal(1, backValue1.Value);
            Assert.Equal(2, backValue2.Value);
        }

        [Fact]
        public void T05_Service_In_Decorator_Async()
        {
            BackValueService backValue1 = new(), backValue2 = new();

            var resolver = DefaultDecoratorsResolver.DecorateAsync<BackValueService, int>((s, i, next) => { s.Value = i; return next(); });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<Task>, Task>;
            Assert.NotNull(function);

            function!(backValue1, 1, () => { backValue2.Value = 2; return Task.CompletedTask; });

            Assert.Equal(1, backValue1.Value);
            Assert.Equal(2, backValue2.Value);
        }

        [Fact]
        public void T06_Service_In_Decorator_Async_WithToken()
        {
            BackValueService backValue1 = new(), backValue2 = new();

            var resolver = DefaultDecoratorsResolver.DecorateAsync<BackValueService, int>((s, i, next, token) => { s.Value = i; return next(); });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<Task>, CancellationToken, Task>;
            Assert.NotNull(function);

            function!(backValue1, 1, () => { backValue2.Value = 2; return Task.CompletedTask; }, default);

            Assert.Equal(1, backValue1.Value);
            Assert.Equal(2, backValue2.Value);
        }


        [Fact]
        public void T07_In_Out_Decorator()
        {
            int backValue1 = 0;
            string backValue2 = string.Empty;

            var resolver = DefaultDecoratorsResolver.Decorate<int, string>((i, next) => { backValue1 = i; return next(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string>, string>;
            Assert.NotNull(function);

            backValue2 = function!(1, () => backValue1.ToString());

            Assert.Equal(1, backValue1);
            Assert.Equal("1", backValue2);
        }

        [Fact]
        public void T08_In_Out_Decorator_Async()
        {
            int backValue1 = 0;
            string backValue2 = string.Empty;

            var resolver = DefaultDecoratorsResolver.DecorateAsync<int, string>((i, next) => { backValue1 = i; return next(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<Task<string>>, Task<string>>;
            Assert.NotNull(function);

            backValue2 = function!(1, () => Task.FromResult(backValue1.ToString())).Result;

            Assert.Equal(1, backValue1);
            Assert.Equal("1", backValue2);
        }

        [Fact]
        public void T09_In_Out_Decorator_Async_WithToken()
        {
            int backValue1 = 0;
            string backValue2 = string.Empty;

            var resolver = DefaultDecoratorsResolver.DecorateAsync<int, string>((i, next, token) => { backValue1 = i; return next(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<Task<string>>, CancellationToken, Task<string>>;
            Assert.NotNull(function);

            backValue2 = function!(1, () => Task.FromResult(backValue1.ToString()), default).Result;

            Assert.Equal(1, backValue1);
            Assert.Equal("1", backValue2);
        }

        [Fact]
        public void T10_Service_In_Out_Decorator()
        {
            BackValueService backValue1 = new();
            string backValue2 = string.Empty;

            var resolver = DefaultDecoratorsResolver.Decorate<BackValueService, int, string>((s, i, next) => { s.Value = i; return next(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<string>, string>;
            Assert.NotNull(function);

            backValue2 = function!(backValue1, 1, () => backValue1.Value.ToString());

            Assert.Equal(1, backValue1.Value);
            Assert.Equal("1", backValue2);
        }

        [Fact]
        public void T11_Service_In_Out_Decorator_Async()
        {
            BackValueService backValue1 = new();
            string backValue2 = string.Empty;

            var resolver = DefaultDecoratorsResolver.DecorateAsync<BackValueService, int, string>((s, i, next) => { s.Value = i; return next(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<Task<string>>, Task<string>>;
            Assert.NotNull(function);

            backValue2 = function!(backValue1, 1, () => Task.FromResult(backValue1.Value.ToString())).Result;

            Assert.Equal(1, backValue1.Value);
            Assert.Equal("1", backValue2);
        }

        [Fact]
        public void T12_Service_In_Out_Decorator_Async_WithToken()
        {
            BackValueService backValue1 = new();
            string backValue2 = string.Empty;

            var resolver = DefaultDecoratorsResolver.DecorateAsync<BackValueService, int, string>((s, i, next, token) => { s.Value = i; return next(); });
            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<BackValueService, int, Func<Task<string>>, CancellationToken, Task<string>>;
            Assert.NotNull(function);

            backValue2 = function!(backValue1, 1, () => Task.FromResult(backValue1.Value.ToString()), default).Result;

            Assert.Equal(1, backValue1.Value);
            Assert.Equal("1", backValue2);
        }

        [Fact]
        public void T13_In_Method_Decorator()
        {
            var type = typeof(GenericMethodDecoratorService<>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodDecoratorResolver(method!);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericMethodDecoratorService<int>, int, Action>;
            Assert.NotNull(action);

            int backValue1 = 0, backValue2 = 0;
            var service = new GenericMethodDecoratorService<int>(i => backValue1 = i);
            action!(service, 1, () => backValue2 = 2);

            Assert.Equal(1, backValue1);
            Assert.Equal(2, backValue2);
        }

        [Fact]
        public void T14_In_Out_Method_Decorator()
        {
            var type = typeof(GenericMethodDecoratorService<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodDecoratorResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodDecoratorService<int, string>, int, Func<string>, string>;
            Assert.NotNull(function);

            int backValue1 = 0;
            string backValue2 = string.Empty;
            var service = new GenericMethodDecoratorService<int, string>(i => backValue1 = i, r => backValue2 = r);
            function!(service, 1, () => backValue1.ToString());

            Assert.Equal(1, backValue1);
            Assert.Equal("1", backValue2);
        }

        [Fact]
        public void T15_In_Method_Decorator_Async()
        {
            var type = typeof(GenericMethodDecoratorServiceAsync<>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodDecoratorResolver(method!);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodDecoratorServiceAsync<int>, int, Func<Task>, Task>;
            Assert.NotNull(function);

            int backValue1 = 0, backValue2 = 0;
            var service = new GenericMethodDecoratorServiceAsync<int>(i => backValue1 = i);
            function!(service, 1, () => { backValue2 = 2; return Task.CompletedTask; });

            Assert.Equal(1, backValue1);
            Assert.Equal(2, backValue2);
        }

        [Fact]
        public void T16_In_Out_Method_Decorator_Async()
        {
            var type = typeof(GenericMethodDecoratorServiceAsync<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodDecoratorResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodDecoratorServiceAsync<int, string>, int, Func<Task<string>>, Task<string>>;
            Assert.NotNull(function);

            int backValue1 = 0;
            string backValue2 = string.Empty;
            var service = new GenericMethodDecoratorServiceAsync<int, string>(i => backValue1 = i, r => backValue2 = r);
            function!(service, 1, () => Task.FromResult(backValue1.ToString()));

            Assert.Equal(1, backValue1);
            Assert.Equal("1", backValue2);
        }

        [Fact]
        public void T17_In_Method_Decorator_Async_WithToken()
        {
            var type = typeof(GenericMethodDecoratorServiceAsyncWithToken<>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodDecoratorResolver(method!);

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodDecoratorServiceAsyncWithToken<int>, int, Func<Task>, CancellationToken, Task>;
            Assert.NotNull(function);

            int backValue1 = 0, backValue2 = 0;
            var service = new GenericMethodDecoratorServiceAsyncWithToken<int>(i => backValue1 = i);
            function!(service, 1, () => { backValue2 = 2; return Task.CompletedTask; }, default);

            Assert.Equal(1, backValue1);
            Assert.Equal(2, backValue2);
        }

        [Fact]
        public void T18_In_Out_Method_Decorator_Async_WithToken()
        {
            var type = typeof(GenericMethodDecoratorServiceAsyncWithToken<,>);
            var method = type.GetMethod("Handle");
            Assert.NotNull(method);
            var resolver = new MethodDecoratorResolver(method!);

            var description = resolver.TryResolve(typeof(int), typeof(string));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(string));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericMethodDecoratorServiceAsyncWithToken<int, string>, int, Func<Task<string>>, CancellationToken, Task<string>>;
            Assert.NotNull(function);

            int backValue1 = 0;
            string backValue2 = string.Empty;
            var service = new GenericMethodDecoratorServiceAsyncWithToken<int, string>(i => backValue1 = i, r => backValue2 = r);
            function!(service, 1, () => Task.FromResult(backValue1.ToString()), default);

            Assert.Equal(1, backValue1);
            Assert.Equal("1", backValue2);
        }

        private class BackValueService
        {
            public int Value { get; set; }

            public BackValueService(int value = 0)
            {
                Value = value;
            }
        }

        private class GenericMethodDecoratorService<TIn>
        {
            private readonly Action<TIn> testCallback;

            public GenericMethodDecoratorService(Action<TIn> testCallback)
            {
                this.testCallback = testCallback ?? throw new ArgumentNullException(nameof(testCallback));
            }

            public void Handle(TIn input, Action next)
            {
                testCallback(input);
                next();
            }
        }

        private class GenericMethodDecoratorService<TIn, TOut>
        {
            private readonly Action<TIn> testCallbackInput;
            private readonly Action<TOut> testCallbackOutput;

            public GenericMethodDecoratorService(Action<TIn> testCallbackInput, Action<TOut> testCallbackOutput)
            {
                this.testCallbackInput = testCallbackInput;
                this.testCallbackOutput = testCallbackOutput;
            }

            public TOut Handle(TIn input, Func<TOut> next)
            {
                testCallbackInput(input);
                var result = next();
                testCallbackOutput(result);
                return result;
            }
        }

        private class GenericMethodDecoratorServiceAsync<TIn>
        {
            private readonly Action<TIn> testCallback;

            public GenericMethodDecoratorServiceAsync(Action<TIn> testCallback)
            {
                this.testCallback = testCallback ?? throw new ArgumentNullException(nameof(testCallback));
            }

            public Task Handle(TIn input, Func<Task> next)
            {
                testCallback(input);
                return next();
            }
        }

        private class GenericMethodDecoratorServiceAsync<TIn, TOut>
        {
            private readonly Action<TIn> testCallbackInput;
            private readonly Action<TOut> testCallbackOutput;

            public GenericMethodDecoratorServiceAsync(Action<TIn> testCallbackInput, Action<TOut> testCallbackOutput)
            {
                this.testCallbackInput = testCallbackInput;
                this.testCallbackOutput = testCallbackOutput;
            }

            public Task<TOut> Handle(TIn input, Func<Task<TOut>> next)
            {
                testCallbackInput(input);
                var resultTask = next();
                testCallbackOutput(resultTask.Result);
                return resultTask;
            }
        }

        private class GenericMethodDecoratorServiceAsyncWithToken<TIn>
        {
            private readonly Action<TIn> testCallback;

            public GenericMethodDecoratorServiceAsyncWithToken(Action<TIn> testCallback)
            {
                this.testCallback = testCallback ?? throw new ArgumentNullException(nameof(testCallback));
            }

            public Task Handle(TIn input, Func<Task> next, CancellationToken cancellationToken)
            {
                testCallback(input);
                return next();
            }
        }

        private class GenericMethodDecoratorServiceAsyncWithToken<TIn, TOut>
        {
            private readonly Action<TIn> testCallbackInput;
            private readonly Action<TOut> testCallbackOutput;

            public GenericMethodDecoratorServiceAsyncWithToken(Action<TIn> testCallbackInput, Action<TOut> testCallbackOutput)
            {
                this.testCallbackInput = testCallbackInput;
                this.testCallbackOutput = testCallbackOutput;
            }

            public Task<TOut> Handle(TIn input, Func<Task<TOut>> next, CancellationToken cancellationToken)
            {
                testCallbackInput(input);
                var resultTask = next();
                testCallbackOutput(resultTask.Result);
                return resultTask;
            }
        }
    }
}
