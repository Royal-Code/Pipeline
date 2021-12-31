using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    [Collection("Pipeline tests")]
    public class T10_PipelineFactory_Decorators_Tests
    {
        [Fact]
        public void T01_DecoratorsHandler_In()
        {
            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddHandlersMethods(MethodsGetter<ValueableHandler<IntInput, int>>.GetSyncMethods());
                    builder.AddDecoratorsMethods(MethodsGetter<DecoratorHandler_In>.GetSyncMethods());
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            pipeline.Send(new IntInput(1));
            Assert.Equal(4, ValueableHandler<IntInput, int>.Value);
        }

        [Fact]
        public async Task T02_DecoratorsHandlerAsync_In()
        {
            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddHandlersMethods(MethodsGetter<ValueableHandler<IntInput, int>>.GetAsyncMethods());
                    builder.AddDecoratorsMethods(MethodsGetter<DecoratorHandlerAsync_In>.GetAsyncMethods());
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            await pipeline.SendAsync(new IntInput(1));
            Assert.Equal(4, ValueableHandler<IntInput, int>.Value);
        }

        [Fact]
        public async Task T03_DecoratorsHandlerAsyncWithoutCancellationToken_In()
        {
            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddHandlersMethods(MethodsGetter<ValueableHandler<IntInput, int>>.GetAsyncMethods());
                    builder.AddDecoratorsMethods(MethodsGetter<DecoratorHandlerAsyncWithoutCancellationToken_In>.GetAsyncMethods());
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            await pipeline.SendAsync(new IntInput(1));
            Assert.Equal(4, ValueableHandler<IntInput, int>.Value);
        }

        [Fact]
        public void T04_DecoratorsHandler_InOut()
        {
            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddHandlersMethods(MethodsGetter<ResultableHandler<IntInput, int>>.GetSyncMethods());
                    builder.AddDecoratorsMethods(MethodsGetter<DecoratorHandler_InOut>.GetSyncMethods());
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.Send(new IntInput(1));
            Assert.Equal(4, ResultableHandler<IntInput, int>.Value);
            Assert.Equal("456", result);
        }

        [Fact]
        public async Task T05_DecoratorsHandlerAsync_InOut()
        {
            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddHandlersMethods(MethodsGetter<ResultableHandler<IntInput, int>>.GetAsyncMethods());
                    builder.AddDecoratorsMethods(MethodsGetter<DecoratorHandlerAsync_InOut>.GetAsyncMethods());
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = await pipeline.SendAsync(new IntInput(1));
            Assert.Equal(4, ResultableHandler<IntInput, int>.Value);
            Assert.Equal("456", result);
        }

        [Fact]
        public async Task T06_DecoratorsHandlerAsyncWithoutCancellationToken_InOut()
        {
            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddHandlersMethods(MethodsGetter<ResultableHandler<IntInput, int>>.GetAsyncMethods());
                    builder.AddDecoratorsMethods(MethodsGetter<DecoratorHandlerAsyncWithoutCancellationToken_InOut>.GetAsyncMethods());
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = await pipeline.SendAsync(new IntInput(1));
            Assert.Equal(4, ResultableHandler<IntInput, int>.Value);
            Assert.Equal("456", result);
        }

        [Fact]
        public void T07_DecoratorsDelegateHandler_In()
        {
            int value = 0;

            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput>()
                        .Handle(i => { i.Increment(); value = i.Value; })
                        .Decorate((i, n) => { i.Increment(); n(); })
                        .Decorate((i, n) => { i.Increment(); n(); });
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            pipeline.Send(new IntInput(1));
            Assert.Equal(4, value);
        }

        internal interface ITestBus_T08 : ITestBus { } 
        [Fact]
        public async Task T08_DecoratorsDelegateHandlerAsync_In()
        {
            PipelineFactory.ResetChainTypes<ITestBus_T08>();

            int value = 0;

            var factory = PipelineFactory.Configure<ITestBus_T08>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput>()
                        .HandleAsync((i, t) => { i.Increment(); value = i.Value; return Task.CompletedTask; })
                        .DecorateAsync((i, n, t) => { i.Increment(); return n(); })
                        .DecorateAsync((i, n, t) => { i.Increment(); return n(); });
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            await pipeline.SendAsync(new IntInput(1));
            await Task.Delay(1);
            Assert.Equal(4, value);
        }

        [Fact]
        public async Task T09_DecoratorsDelegateHandlerAsyncWithoutCancellationToken_In()
        {
            int value = 0;

            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput>()
                        .HandleAsync((i) => { i.Increment(); value = i.Value; return Task.CompletedTask; })
                        .DecorateAsync((i, n) => { i.Increment(); return n(); })
                        .DecorateAsync((i, n) => { i.Increment(); return n(); });
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            await pipeline.SendAsync(new IntInput(1));
            Assert.Equal(4, value);
        }

        [Fact]
        public void T10_DecoratorsDelegateHandler_InOut()
        {
            int value = 0;

            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput, string>()
                        .Handle(i => { i.Increment(); value = i.Value; return i.ToString(); })
                        .Decorate((i, n) => { i.Increment(); var r = n(); i.Increment(); return r + i.ToString(); })
                        .Decorate((i, n) => { i.Increment(); var r = n(); i.Increment(); return r + i.ToString(); });
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.Send(new IntInput(1));
            Assert.Equal(4, value);
            Assert.Equal("456", result);
        }

        [Fact]
        public async void T11_DecoratorsDelegateHandlerAsync_InOut()
        {
            int value = 0;

            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput, string>()
                        .HandleAsync((i, t) => { i.Increment(); value = i.Value; return Task.FromResult(i.ToString()); })
                        .DecorateAsync(async (i, n, t) => { i.Increment(); var r = await n(); i.Increment(); return r + i.ToString(); })
                        .DecorateAsync(async (i, n, t) => { i.Increment(); var r = await n(); i.Increment(); return r + i.ToString(); });
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = await pipeline.SendAsync(new IntInput(1));
            Assert.Equal(4, value);
            Assert.Equal("456", result);
        }

        [Fact]
        public async void T12_DecoratorsDelegateHandlerAsyncWithoutCancellationToken_InOut()
        {
            int value = 0;

            PipelineFactory.ResetChainTypes<ITestBus>();
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput, string>()
                        .HandleAsync((i) => { i.Increment(); value = i.Value; return Task.FromResult(i.ToString()); })
                        .DecorateAsync(async (i, n) => { i.Increment(); var r = await n(); i.Increment(); return r + i.ToString(); })
                        .DecorateAsync(async (i, n) => { i.Increment(); var r = await n(); i.Increment(); return r + i.ToString(); });
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = await pipeline.SendAsync(new IntInput(1));
            Assert.Equal(4, value);
            Assert.Equal("456", result);
        }
    }

    #region Input & Handlers

    public interface IValueable<TValue>
    {
        TValue Value { get; }
        void Increment();
    }

    public class IntInput : IValueable<int>
    {
        public int Value { get; set; }

        public IntInput(int value)
        {
            Value = value;
        }

        public void Increment()
        {
            Value++;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class ValueableHandler<TValueable, TValue>
        where TValueable : IValueable<TValue>
    {
        public static TValue? Value;

        public void Handle(TValueable input)
        {
            input.Increment();
            Value = input.Value;
        }

        public Task HandleAsync(TValueable input)
        {
            input.Increment();
            Value = input.Value;
            return Task.CompletedTask;
        }
    }

    public class ResultableHandler<TValueable, TValue>
        where TValueable : IValueable<TValue>
    {
        public static TValue? Value;

        public string Handle(TValueable input)
        {
            input.Increment();
            Value = input.Value;
            return input.ToString()!;
        }

        public Task<string> HandleAsync(TValueable input)
        {
            input.Increment();
            Value = input.Value;
            return Task.FromResult(input.ToString()!);
        }
    }

    public class MethodsGetter<T>
    {
        public static MethodInfo[] GetSyncMethods() => typeof(T)
            .GetMethods()
            .Where(m => m.Name.Contains("Handle", StringComparison.OrdinalIgnoreCase)
                && !m.Name.Contains("Async", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        public static MethodInfo[] GetAsyncMethods() => typeof(T)
            .GetMethods()
            .Where(m => m.Name.Contains("Handle", StringComparison.OrdinalIgnoreCase)
                && m.Name.Contains("Async", StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    #endregion

    #region T01 & T02 & T03

    public class DecoratorHandler_In
    {
        public void Handle1(IntInput input, Action next)
        {
            input.Increment();
            next();
        }

        public void Handle2(IntInput input, Action next)
        {
            input.Increment();
            next();
        }
    }

    public class DecoratorHandlerAsync_In
    {
        public Task HandleAsync1(IntInput input, Func<Task> next, CancellationToken token)
        {
            input.Increment();
            return next();
        }

        public Task HandleAsync2(IntInput input, Func<Task> next, CancellationToken token)
        {
            input.Increment();
            return next();
        }
    }

    public class DecoratorHandlerAsyncWithoutCancellationToken_In
    {
        public Task HandleAsync1(IntInput input, Func<Task> next)
        {
            input.Increment();
            return next();
        }

        public Task HandleAsync2(IntInput input, Func<Task> next)
        {
            input.Increment();
            return next();
        }
    }

    #endregion

    #region T04 & T05 & T06

    public class DecoratorHandler_InOut
    {
        public string Handle1(IntInput input, Func<string> next)
        {
            input.Increment();
            var result = next();
            input.Increment();
            return result + input.ToString();
        }

        public string Handle2(IntInput input, Func<string> next)
        {
            input.Increment();
            var result = next();
            input.Increment();
            return result + input.ToString();
        }
    }

    public class DecoratorHandlerAsync_InOut
    {
        public async Task<string> HandleAsync1(IntInput input, Func<Task<string>> next, CancellationToken token)
        {
            input.Increment();
            var result = await next();
            input.Increment();
            return result + input.ToString();
        }

        public async Task<string> HandleAsync2(IntInput input, Func<Task<string>> next, CancellationToken token)
        {
            input.Increment();
            var result = await next();
            input.Increment();
            return result + input.ToString();
        }
    }

    public class DecoratorHandlerAsyncWithoutCancellationToken_InOut
    {
        public async Task<string> HandleAsync1(IntInput input, Func<Task<string>> next)
        {
            input.Increment();
            var result = await next();
            input.Increment();
            return result + input.ToString();
        }

        public async Task<string> HandleAsync2(IntInput input, Func<Task<string>> next)
        {
            input.Increment();
            var result = await next();
            input.Increment();
            return result + input.ToString();
        }
    }

    #endregion
}
