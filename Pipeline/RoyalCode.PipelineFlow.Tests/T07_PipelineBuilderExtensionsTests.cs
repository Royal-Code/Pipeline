using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T07_PipelineBuilderExtensionsTests
    {
        [Fact]
        public void T01_Handle_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.Handle(i => { });

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T02_ServiceHandle_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderHandlerService>()
                .Handle((s, i) => { });

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T03_HandleAsync_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.HandleAsync((i, t) => Task.CompletedTask);

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T04_HandleAsyncWithoutCancellationToken_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.HandleAsync(i => Task.CompletedTask);

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T05_ServiceHandleAsync_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderHandlerService>()
                .HandleAsync((s, i, t) => Task.CompletedTask);

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T06_ServiceHandleAsyncWithoutCancellationToken_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderHandlerService>()
                .HandleAsync((s, i) => Task.CompletedTask);

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T07_Handle_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.Handle(i => i.ToString());

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T08_ServiceHandle_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderHandlerService>()
                .Handle((s, i) => i.ToString());

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T09_HandleAsync_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.HandleAsync((i, t) => Task.FromResult(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T10_HandleAsyncWithoutCancellationToken_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.HandleAsync(i => Task.FromResult(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T11_ServiceHandleAsync_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderHandlerService>()
                .HandleAsync((s, i, t) => Task.FromResult(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T12_ServiceHandleAsyncWithoutCancellationToken_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderHandlerService>()
                .HandleAsync((s, i) => Task.FromResult(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T13_MethodHandle_SingleMethod()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.Handle(PipelineBuilderHandlerService.GetHandlerMethod());

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T14_MethodHandle_ArrayOfMethods()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddHandlersMethods(PipelineBuilderHandlerService.GetHandlerMethods());

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
            description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T15_MethodHandle_ByAttribute()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddHandlersMethodsDefined<PipelineBuilderHandlerService, TestPipelineBuilderAttribute>();

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
            description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.Null(description);
        }

        [Fact]
        public void T16_MethodHandle_ByName()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddHandlerMethodDefined<PipelineBuilderHandlerService>(nameof(PipelineBuilderHandlerService.HandleAsync));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T17_Bridge_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.BridgeHandle<int, string>((i, n) => n(i.ToString()));
            
            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T18_BridgeAsync_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.BridgeHandleAsync<int, string>((i, n, t) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T19_BridgeAsyncWithoutCancellationToken_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.BridgeHandleAsync<int, string>((i, n) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T20_ServiceBridge_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderBridgeHandlerService>()
                .BridgeHandle<PipelineBuilderBridgeHandlerService, int, string>((s, i, n) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T21_ServiceBridgeAsync_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderBridgeHandlerService>()
                .BridgeHandleAsync<PipelineBuilderBridgeHandlerService, int, string>((s, i, n, t) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T22_ServiceBridgeAsyncWithoutCancellationToken_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderBridgeHandlerService>()
                .BridgeHandleAsync<PipelineBuilderBridgeHandlerService, int, string>((s, i, n) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T23_Bridge_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.BridgeHandle<int, string, string>((i, n) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T24_BridgeAsync_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.BridgeHandleAsync<int, string, string>((i, n, t) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T25_BridgeAsyncWithoutCancellationToken_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.BridgeHandleAsync<int, string, string>((i, n) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T26_ServiceBridge_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderBridgeHandlerService>()
                .BridgeHandle<PipelineBuilderBridgeHandlerService, int, string, string>((s, i, n) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T27_ServiceBridgeAsync_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderBridgeHandlerService>()
                .BridgeHandleAsync<PipelineBuilderBridgeHandlerService, int, string, string>((s, i, n, t) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T28_ServiceBridgeAsyncWithoutCancellationToken_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderBridgeHandlerService>()
                .BridgeHandleAsync<PipelineBuilderBridgeHandlerService, int, string, string>((s, i, n) => n(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T29_Bridge_InOutNext()
        {
            var builder = TestPipelineBuilder.Create<int, decimal>();

            builder.BridgeHandle<int, decimal, string, string>((i, n) => decimal.Parse(n(i.ToString())));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(decimal));
            Assert.NotNull(description);
        }

        [Fact]
        public void T30_BridgeAsync_InOutNext()
        {
            var builder = TestPipelineBuilder.Create<int, decimal>();

            builder.BridgeHandleAsync<int, decimal, string, string>(async (i, n, t) => decimal.Parse(await n(i.ToString())));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(decimal));
            Assert.NotNull(description);
        }

        [Fact]
        public void T31_BridgeAsyncWithoutCancellationToken_InOutNext()
        {
            var builder = TestPipelineBuilder.Create<int, decimal>();

            builder.BridgeHandleAsync<int, decimal, string, string>(async (i, n) => decimal.Parse(await n(i.ToString())));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(decimal));
            Assert.NotNull(description);
        }

        [Fact]
        public void T32_ServiceBridge_InOutNext()
        {
            var builder = TestPipelineBuilder.Create<int, decimal>();

            builder.WithService<PipelineBuilderBridgeHandlerService>()
                .BridgeHandle<PipelineBuilderBridgeHandlerService, int, decimal, string, string>((s, i, n) => decimal.Parse(n(i.ToString())));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(decimal));
            Assert.NotNull(description);
        }

        [Fact]
        public void T33_ServiceBridgeAsync_InOutNext()
        {
            var builder = TestPipelineBuilder.Create<int, decimal>();

            builder.WithService<PipelineBuilderBridgeHandlerService>()
                .BridgeHandleAsync<PipelineBuilderBridgeHandlerService, int, decimal, string, string>(async (s, i, n, t) => decimal.Parse(await n(i.ToString())));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(decimal));
            Assert.NotNull(description);
        }

        [Fact]
        public void T34_ServiceBridgeAsyncWithoutCancellationToken_InOutNext()
        {
            var builder = TestPipelineBuilder.Create<int, decimal>();

            builder.WithService<PipelineBuilderBridgeHandlerService>()
                .BridgeHandleAsync<PipelineBuilderBridgeHandlerService, int, decimal, string, string>(async (s, i, n) => decimal.Parse(await n(i.ToString())));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(decimal));
            Assert.NotNull(description);
        }

        [Fact]
        public void T35_MethodBridge_SingleMethod()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.BridgeHandle(PipelineBuilderBridgeHandlerService.GetHandlerMethod());

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T36_MethodBridge_ArrayOfMethods()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddBridgeHandlersMethods(PipelineBuilderBridgeHandlerService.GetHandlerMethods());

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
            description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
            description = builder.Handlers.GetDescription(typeof(long), typeof(decimal));
            Assert.NotNull(description);
        }

        [Fact]
        public void T37_MethodBridge_ByAttribute()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddBridgeHandlersMethodsDefined<PipelineBuilderBridgeHandlerService, TestPipelineBuilderAttribute>();

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
            description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.Null(description);
            description = builder.Handlers.GetDescription(typeof(long), typeof(decimal));
            Assert.Null(description);
        }

        [Fact]
        public void T38_MethodBridge_ByName()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddBridgeHandlerMethodDefined<PipelineBuilderBridgeHandlerService>(nameof(PipelineBuilderBridgeHandlerService.Handle));

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T39_MethodBridge_ByName_Throws()
        {
            var builder = TestPipelineBuilder.Create<int>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                builder.AddBridgeHandlerMethodDefined<PipelineBuilderBridgeHandlerService>(nameof(PipelineBuilderBridgeHandlerService.HandleAsync));
            });
        }

        [Fact]
        public void T40_Decorate_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.Decorate((i, n) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T41_DecorateAsync_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.DecorateAsync((i, n, t) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T42_DecorateAsyncWithoutCancellationToken_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.DecorateAsync((i, n) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T43_ServiceDecorate_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderDecoratorService>()
                .Decorate((s, i, n) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T44_ServiceDecorateAsync_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderDecoratorService>()
                .DecorateAsync((s, i, n, t) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T45_ServiceDecorateAsyncWithoutCancellationToken_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderDecoratorService>()
                .DecorateAsync((s, i, n) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T46_Decorate_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.Decorate((i, n) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int), typeof(string));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T47_DecorateAsync_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.DecorateAsync((i, n, t) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int), typeof(string));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T48_DecorateAsyncWithoutCancellationToken_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.DecorateAsync((i, n) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int), typeof(string));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T49_ServiceDecorate_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderDecoratorService>()
                .Decorate((s, i, n) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int), typeof(string));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T50_ServiceDecorateAsync_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderDecoratorService>()
                .DecorateAsync((s, i, n, t) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int), typeof(string));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T51_ServiceDecorateAsyncWithoutCancellationToken_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderDecoratorService>()
                .DecorateAsync((s, i, n) => n());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int), typeof(string));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T52_MethodDecorate_SingleMethod()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.Decorate(PipelineBuilderDecoratorService.GetHandlerMethod());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T53_MethodDecorate_ArrayOfMethods()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddDecoratorsMethods(PipelineBuilderDecoratorService.GetHandlerMethods());

            var descriptions = builder.Decorators.GetDescriptions(typeof(int));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
            descriptions = builder.Decorators.GetDescriptions(typeof(int), typeof(string));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }

        [Fact]
        public void T54_MethodDecorate_ByAttribute()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddDecoratorsMethodsDefined<PipelineBuilderDecoratorService, TestPipelineBuilderAttribute>();

            var descriptions = builder.Decorators.GetDescriptions(typeof(int));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
            descriptions = builder.Decorators.GetDescriptions(typeof(int), typeof(string));
            Assert.NotNull(descriptions);
            Assert.Empty(descriptions);
        }

        [Fact]
        public void T55_MethodDecorate_ByName()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddDecoratorMethodDefined<PipelineBuilderDecoratorService>(nameof(PipelineBuilderDecoratorService.HandleAsync));

            var descriptions = builder.Decorators.GetDescriptions(typeof(int), typeof(string));
            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
        }
    }

    internal class PipelineBuilderRegistry : IPipelineConfiguration
    {
        public HandlerRegistry Handlers { get; } = new HandlerRegistry();

        public DecoratorRegistry Decorators { get; } = new DecoratorRegistry();
    }

    internal class TestPipelineBuilder : DefaultPipelineBuilder
    {
        public TestPipelineBuilder(PipelineBuilderRegistry registry) 
            : base(registry)
        {
            Registry = registry;
        }

        public PipelineBuilderRegistry Registry { get; }

        public static TestPipelineBuilder<TIn> Create<TIn>()
        {
            var registry = new PipelineBuilderRegistry();
            var builder = new TestPipelineBuilder(registry);
            return new TestPipelineBuilder<TIn>(builder);
        }

        public static TestPipelineBuilder<TIn, TOut> Create<TIn, TOut>()
        {
            var registry = new PipelineBuilderRegistry();
            var builder = new TestPipelineBuilder(registry);
            return new TestPipelineBuilder<TIn, TOut>(builder);
        }
    }

    internal class TestPipelineBuilder<TIn> : DefaultPipelineBuilder<TIn>
    {
        private readonly PipelineBuilderRegistry registry;

        public TestPipelineBuilder(TestPipelineBuilder pipelineBuilder) 
            : base(pipelineBuilder)
        {
            registry = pipelineBuilder.Registry;
        }

        public HandlerRegistry Handlers => registry.Handlers;

        public DecoratorRegistry Decorators => registry.Decorators;
    }

    internal class TestPipelineBuilder<TIn, TOut> : DefaultPipelineBuilder<TIn, TOut>
    {
        private readonly PipelineBuilderRegistry registry;

        public TestPipelineBuilder(TestPipelineBuilder pipelineBuilder)
            : base(pipelineBuilder)
        {
            registry = pipelineBuilder.Registry;
        }

        public HandlerRegistry Handlers => registry.Handlers;

        public DecoratorRegistry Decorators => registry.Decorators;
    }

    internal class PipelineBuilderHandlerService 
    {
        public static MethodInfo GetHandlerMethod() => typeof(PipelineBuilderHandlerService)
            .GetMethod(nameof(PipelineBuilderHandlerService.Handle))!;

        public static MethodInfo[] GetHandlerMethods() => typeof(PipelineBuilderHandlerService)
            .GetMethods()
            .Where(m => m.Name 
                is nameof(PipelineBuilderHandlerService.Handle)
                or nameof(PipelineBuilderHandlerService.HandleAsync))
            .ToArray();

        [TestPipelineBuilder]
        public void Handle(int input) { }

        public Task<string> HandleAsync(int input, CancellationToken token) => Task.FromResult(input.ToString());
    }

    internal class PipelineBuilderBridgeHandlerService
    {
        public static MethodInfo GetHandlerMethod() => typeof(PipelineBuilderBridgeHandlerService)
            .GetMethod(nameof(PipelineBuilderBridgeHandlerService.Handle))!;

        public static MethodInfo[] GetHandlerMethods() => typeof(PipelineBuilderBridgeHandlerService)
            .GetMethods()
            .Where(m => m.Name
                is nameof(PipelineBuilderBridgeHandlerService.Handle)
                or nameof(PipelineBuilderBridgeHandlerService.HandleAsync))
            .ToArray();

        [TestPipelineBuilder]
        public void Handle(int input, Action<string> next) => next(input.ToString());

        public Task<string> HandleAsync(int input, Func<string, Task<string>> next, CancellationToken token) => next(input.ToString());

        public async Task<decimal> HandleAsync(long input, Func<string, Task<string>> next, CancellationToken token)
        {
            var result = await next(input.ToString());
            return decimal.Parse(result);
        }
    }

    internal class PipelineBuilderDecoratorService
    {
        public static MethodInfo GetHandlerMethod() => typeof(PipelineBuilderDecoratorService)
            .GetMethod(nameof(PipelineBuilderDecoratorService.Handle))!;

        public static MethodInfo[] GetHandlerMethods() => typeof(PipelineBuilderDecoratorService)
            .GetMethods()
            .Where(m => m.Name
                is nameof(PipelineBuilderDecoratorService.Handle)
                or nameof(PipelineBuilderDecoratorService.HandleAsync))
            .ToArray();

        [TestPipelineBuilder]
        public void Handle(int input, Action next) => next();

        public Task<string> HandleAsync(int input, Func<Task<string>> next, CancellationToken token) => next();
    }

    internal class TestPipelineBuilderAttribute : Attribute { }
}
