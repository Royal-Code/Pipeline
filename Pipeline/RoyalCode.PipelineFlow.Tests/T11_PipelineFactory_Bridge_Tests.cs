using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T11_PipelineFactory_Bridge_Tests
    {

        [Fact]
        public void T01_BridgeDelegateHandler_In()
        {
            int intValue = 0;
            string stringValue = string.Empty;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput>()
                        .Decorate((i, n) => { i.Increment(); n(); })
                        .BridgeHandle<IntInput, StringInput>((i, n) => { i.Increment(); intValue = i.Value; n(new StringInput(i)); });

                    builder.Configure<StringInput>()
                        .Decorate((i, n) => { i.Increment(); n(); })
                        .Handle(i => { i.Increment(); stringValue = i.Value; });
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            pipeline.Send(new IntInput(1));
            Assert.Equal(3, intValue);
            Assert.Equal("3333", stringValue);
        }

        [Fact]
        public void T02_BridgeDelegateHandlerAsync_In()
        {
            int intValue = 0;
            string stringValue = string.Empty;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput>()
                        .DecorateAsync((i, n, t) => { i.Increment(); return n(); })
                        .BridgeHandleAsync<IntInput, StringInput>((i, n, t) => { i.Increment(); intValue = i.Value; return n(new StringInput(i)); });

                    builder.Configure<StringInput>()
                        .DecorateAsync((i, n, t) => { i.Increment(); return n(); })
                        .HandleAsync((i, t) => { i.Increment(); stringValue = i.Value; return Task.CompletedTask; });
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, intValue);
            Assert.Equal("3333", stringValue);
        }

        [Fact]
        public void T03_BridgeDelegateHandlerAsyncWithoutCancellationToken_In()
        {
            int intValue = 0;
            string stringValue = string.Empty;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput>()
                        .DecorateAsync((i, n) => { i.Increment(); return n(); })
                        .BridgeHandleAsync<IntInput, StringInput>((i, n) => { i.Increment(); intValue = i.Value; return n(new StringInput(i)); });

                    builder.Configure<StringInput>()
                        .DecorateAsync((i, n) => { i.Increment(); return n(); })
                        .HandleAsync((i) => { i.Increment(); stringValue = i.Value; return Task.CompletedTask; });
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, intValue);
            Assert.Equal("3333", stringValue);
        }

        [Fact]
        public void T04_BridgeDelegateHandler_InOut()
        {
            int intValue = 0;
            string stringValue = string.Empty;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput, string>()
                        .Decorate((i, n) => { i.Increment(); return n(); })
                        .BridgeHandle<IntInput, string, StringInput>((i, n) => { i.Increment(); intValue = i.Value; return n(new StringInput(i)); });

                    builder.Configure<StringInput, string>()
                        .Decorate((i, n) => { i.Increment(); return n(); })
                        .Handle(i => { i.Increment(); stringValue = i.Value; return i.Value; });
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.Send(new IntInput(1));
            Assert.Equal(3, intValue);
            Assert.Equal("3333", stringValue);
            Assert.Equal("3333", result);
        }

        [Fact]
        public void T05_BridgeDelegateHandlerAsync_InOut()
        {
            int intValue = 0;
            string stringValue = string.Empty;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput, string>()
                        .DecorateAsync((i, n, t) => { i.Increment(); return n(); })
                        .BridgeHandleAsync<IntInput, string, StringInput>((i, n, t) => { i.Increment(); intValue = i.Value; return n(new StringInput(i)); });

                    builder.Configure<StringInput, string>()
                        .DecorateAsync((i, n, t) => { i.Increment(); return n(); })
                        .HandleAsync((i, t) => { i.Increment(); stringValue = i.Value; return Task.FromResult(i.Value); });
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, intValue);
            Assert.Equal("3333", stringValue);
            Assert.Equal("3333", result);
        }

        [Fact]
        public void T06_BridgeDelegateHandlerAsyncWithoutCancellationToken_InOut()
        {
            int intValue = 0;
            string stringValue = string.Empty;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput, string>()
                        .DecorateAsync((i, n) => { i.Increment(); return n(); })
                        .BridgeHandleAsync<IntInput, string, StringInput>((i, n) => { i.Increment(); intValue = i.Value; return n(new StringInput(i)); });

                    builder.Configure<StringInput, string>()
                        .DecorateAsync((i, n) => { i.Increment(); return n(); })
                        .HandleAsync((i) => { i.Increment(); stringValue = i.Value; return Task.FromResult(i.Value); });
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, intValue);
            Assert.Equal("3333", stringValue);
            Assert.Equal("3333", result);
        }

        [Fact]
        public void T07_BridgeDelegateHandler_InOutNext()
        {
            int intValue = 0;
            string stringValue = string.Empty;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput, string>()
                        .Decorate((i, n) => { i.Increment(); return n(); })
                        .BridgeHandle<IntInput, string, StringInput, MultiInput>((i, n) => 
                        { 
                            i.Increment(); 
                            intValue = i.Value; 
                            var r = n(new StringInput(i)); 
                            return r.ToString()!; 
                        });

                    builder.Configure<StringInput, MultiInput>()
                        .Decorate((i, n) => { i.Increment(); var r = n(); r.Increment(); return r; })
                        .Handle(i => { i.Increment(); stringValue = i.Value; return new MultiInput(i.Value); });
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.Send(new IntInput(1));
            Assert.Equal(3, intValue);
            Assert.Equal("3333", stringValue);
            Assert.Equal("6666", result);
        }

        [Fact]
        public void T08_BridgeDelegateHandlerAsync_InOutNext()
        {
            int intValue = 0;
            string stringValue = string.Empty;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput, string>()
                        .DecorateAsync((i, n, t) => { i.Increment(); return n(); })
                        .BridgeHandleAsync<IntInput, string, StringInput, MultiInput>((i, n, t) =>
                        {
                            i.Increment();
                            intValue = i.Value;
                            var r = n(new StringInput(i)).Result;
                            return Task.FromResult(r.ToString()!);
                        });

                    builder.Configure<StringInput, MultiInput>()
                        .DecorateAsync((i, n, t) => { i.Increment(); var r = n().Result; r.Increment(); return Task.FromResult(r); })
                        .HandleAsync((i, t) => { i.Increment(); stringValue = i.Value; return Task.FromResult(new MultiInput(i.Value)); });
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.SendAsync(new IntInput(1)).Result;
            Assert.Equal(3, intValue);
            Assert.Equal("3333", stringValue);
            Assert.Equal("6666", result);
        }

        [Fact]
        public void T09_BridgeDelegateHandlerAsyncWithoutCancellationToken_InOutNext()
        {
            int intValue = 0;
            string stringValue = string.Empty;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.Configure<IntInput, string>()
                        .DecorateAsync((i, n) => { i.Increment(); return n(); })
                        .BridgeHandleAsync<IntInput, string, StringInput, MultiInput>((i, n) =>
                        {
                            i.Increment();
                            intValue = i.Value;
                            var r = n(new StringInput(i)).Result;
                            return Task.FromResult(r.ToString()!);
                        });

                    builder.Configure<StringInput, MultiInput>()
                        .DecorateAsync((i, n) => { i.Increment(); var r = n().Result; r.Increment(); return Task.FromResult(r); })
                        .HandleAsync((i) => { i.Increment(); stringValue = i.Value; return Task.FromResult(new MultiInput(i.Value)); });
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.SendAsync(new IntInput(1)).Result;
            Assert.Equal(3, intValue);
            Assert.Equal("3333", stringValue);
            Assert.Equal("6666", result);
        }

        [Fact]
        public void T10_BridgeHandler_In()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddDecoratorsMethods(Decorators.GetMethods());
                    builder.BridgeHandle(Bridges.GetMethod(nameof(Bridges.HandleNextStringInput)));
                    builder.Handle(Handlers.GetMethod(nameof(Handlers.HandleStringInput)));
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            pipeline.Send(new IntInput(1));
            Assert.Equal(3, Bridges.IntValue);
            Assert.Equal("3333", Handlers.StringValue);
        }

        [Fact]
        public void T11_BridgeHandlerAsync_In()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddDecoratorsMethods(Decorators.GetMethods());
                    builder.BridgeHandle(Bridges.GetMethod(nameof(Bridges.HandleNextStringInputAsync)));
                    builder.Handle(Handlers.GetMethod(nameof(Handlers.HandleStringInputAsync)));
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, Bridges.IntValue);
            Assert.Equal("3333", Handlers.StringValue);
        }

        [Fact]
        public void T12_BridgeHandlerAsyncWithoutCancellationToken_In()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddDecoratorsMethods(Decorators.GetMethods());
                    builder.BridgeHandle(Bridges.GetMethod(nameof(Bridges.HandleNextStringInputAsyncWithoutCancellationToken)));
                    builder.Handle(Handlers.GetMethod(nameof(Handlers.HandleStringInputAsyncWithoutCancellationToken)));
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, Bridges.IntValue);
            Assert.Equal("3333", Handlers.StringValue);
        }

        [Fact]
        public void T13_BridgeHandler_InOut()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddDecoratorsMethods(Decorators.GetMethods());
                    builder.BridgeHandle(Bridges.GetMethod(nameof(Bridges.HandleNextStringInputResultString)));
                    builder.Handle(Handlers.GetMethod(nameof(Handlers.HandleStringInputResultString)));
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.Send(new IntInput(1));
            Assert.Equal(3, Bridges.IntValue);
            Assert.Equal("3333", Handlers.StringValue);
            Assert.Equal("3333", result);
        }

        [Fact]
        public void T14_BridgeHandlerAsync_InOut()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddDecoratorsMethods(Decorators.GetMethods());
                    builder.BridgeHandle(Bridges.GetMethod(nameof(Bridges.HandleNextStringInputResultStringAsync)));
                    builder.Handle(Handlers.GetMethod(nameof(Handlers.HandleStringInputResultStringAsync)));
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, Bridges.IntValue);
            Assert.Equal("3333", Handlers.StringValue);
            Assert.Equal("3333", result);
        }

        [Fact]
        public void T15_BridgeHandlerAsyncWithoutCancellationToken_InOut()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddDecoratorsMethods(Decorators.GetMethods());
                    builder.BridgeHandle(Bridges.GetMethod(nameof(Bridges.HandleNextStringInputResultStringAsyncWithoutCancellationToken)));
                    builder.Handle(Handlers.GetMethod(nameof(Handlers.HandleStringInputResultStringAsyncWithoutCancellationToken)));
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, Bridges.IntValue);
            Assert.Equal("3333", Handlers.StringValue);
            Assert.Equal("3333", result);
        }

        [Fact]
        public void T16_BridgeHandler_InOutNext()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddDecoratorsMethods(Decorators.GetMethods());
                    builder.BridgeHandle(Bridges.GetMethod(nameof(Bridges.HandleNextStringInputResultMultiInput)));
                    builder.Handle(Handlers.GetMethod(nameof(Handlers.HandleStringInputResultMultiInput)));
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.Send(new IntInput(1));
            Assert.Equal(3, Bridges.IntValue);
            Assert.Equal("3333", Handlers.StringValue);
            Assert.Equal("6666", result);
        }

        [Fact]
        public void T17_BridgeHandlerAsync_InOutNext()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddDecoratorsMethods(Decorators.GetMethods());
                    builder.BridgeHandle(Bridges.GetMethod(nameof(Bridges.HandleNextStringInputResultMultiInputAsync)));
                    builder.Handle(Handlers.GetMethod(nameof(Handlers.HandleStringInputResultMultiInputAsync)));
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, Bridges.IntValue);
            Assert.Equal("3333", Handlers.StringValue);
            Assert.Equal("6666", result);
        }

        [Fact]
        public void T18_BridgeHandlerAsyncWithoutCancellationToken_InOutNext()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddDecoratorsMethods(Decorators.GetMethods());
                    builder.BridgeHandle(Bridges.GetMethod(nameof(Bridges.HandleNextStringInputResultMultiInputAsyncWithoutCancellationToken)));
                    builder.Handle(Handlers.GetMethod(nameof(Handlers.HandleStringInputResultMultiInputAsyncWithoutCancellationToken)));
                })
                .Create();

            var pipeline = factory.Create<IntInput, string>();
            Assert.NotNull(pipeline);

            var result = pipeline.SendAsync(new IntInput(1)).GetAwaiter().GetResult();
            Assert.Equal(3, Bridges.IntValue);
            Assert.Equal("3333", Handlers.StringValue);
            Assert.Equal("6666", result);
        }
    }

    public class StringInput : IValueable<string>
    {
        public string Value { get; private set; }

        public StringInput(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public StringInput(IntInput intInput)
        {
            Value = intInput.ToString();
        }

        public void Increment()
        {
            Value += Value;
        }
    }

    public class MultiInput : IValueable<long>
    {
        public long Value { get; private set; }

        public MultiInput(long value)
        {
            Value = value;
        }

        public MultiInput(string value)
        {
            if (!long.TryParse(value, out var v))
                v = value.Length;
            Value = v;
        }

        public void Increment()
        {
            Value *= 2;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Handlers
    {
        public static string StringValue = string.Empty;

        public static MethodInfo GetMethod(string name)
        {
            return typeof(Handlers).GetMethod(name)
                ?? throw new ArgumentException("Method not found with name " + name, nameof(name));
        }

        public void HandleStringInput(StringInput i)
        {
            i.Increment(); 
            StringValue = i.Value;
        }

        public Task HandleStringInputAsync(StringInput i, CancellationToken cancellationToken)
        {
            i.Increment();
            StringValue = i.Value;
            return Task.CompletedTask;
        }

        public Task HandleStringInputAsyncWithoutCancellationToken(StringInput i)
        {
            i.Increment();
            StringValue = i.Value;
            return Task.CompletedTask;
        }

        public string HandleStringInputResultString(StringInput i)
        {
            i.Increment();
            StringValue = i.Value;
            return i.Value;
        }

        public Task<string> HandleStringInputResultStringAsync(StringInput i, CancellationToken cancellationToken)
        {
            i.Increment();
            StringValue = i.Value;
            return Task.FromResult(i.Value);
        }

        public Task<string> HandleStringInputResultStringAsyncWithoutCancellationToken(StringInput i)
        {
            i.Increment();
            StringValue = i.Value;
            return Task.FromResult(i.Value);
        }

        public MultiInput HandleStringInputResultMultiInput(StringInput i)
        {
            i.Increment();
            StringValue = i.Value;
            return new MultiInput(i.Value);
        }

        public Task<MultiInput> HandleStringInputResultMultiInputAsync(StringInput i, CancellationToken cancellationToken)
        {
            i.Increment();
            StringValue = i.Value;
            return Task.FromResult(new MultiInput(i.Value));
        }

        public Task<MultiInput> HandleStringInputResultMultiInputAsyncWithoutCancellationToken(StringInput i)
        {
            i.Increment();
            StringValue = i.Value;
            return Task.FromResult(new MultiInput(i.Value));
        }
    }

    public class Bridges
    {
        public static int IntValue;

        public static MethodInfo GetMethod(string name)
        {
            return typeof(Bridges).GetMethod(name)
                ?? throw new ArgumentException("Method not found with name " + name, nameof(name));
        }

        public void HandleNextStringInput(IntInput i, Action<StringInput> next)
        {
            i.Increment();
            IntValue = i.Value;
            next(new StringInput(i));
        }

        public Task HandleNextStringInputAsync(IntInput i, Func<StringInput, Task> next, CancellationToken token)
        {
            i.Increment();
            IntValue = i.Value;
            return next(new StringInput(i));
        }

        public Task HandleNextStringInputAsyncWithoutCancellationToken(IntInput i, Func<StringInput, Task> next)
        {
            i.Increment();
            IntValue = i.Value;
            return next(new StringInput(i));
        }

        public string HandleNextStringInputResultString(IntInput i, Func<StringInput, string> next)
        {
            i.Increment();
            IntValue = i.Value;
            return next(new StringInput(i));
        }

        public Task<string> HandleNextStringInputResultStringAsync(IntInput i, Func<StringInput, Task<string>> next, CancellationToken token)
        {
            i.Increment();
            IntValue = i.Value;
            return next(new StringInput(i));
        }

        public Task<string> HandleNextStringInputResultStringAsyncWithoutCancellationToken(IntInput i, Func<StringInput, Task<string>> next)
        {
            i.Increment();
            IntValue = i.Value;
            return next(new StringInput(i));
        }

        public string HandleNextStringInputResultMultiInput(IntInput i, Func<StringInput, MultiInput> next)
        {
            i.Increment();
            IntValue = i.Value;
            var r = next(new StringInput(i));
            return r.ToString()!;
        }

        public Task<string> HandleNextStringInputResultMultiInputAsync(IntInput i, Func<StringInput, Task<MultiInput>> next, CancellationToken token)
        {
            i.Increment();
            IntValue = i.Value;
            var r = next(new StringInput(i)).GetAwaiter().GetResult();
            return Task.FromResult(r.ToString()!);
        }

        public Task<string> HandleNextStringInputResultMultiInputAsyncWithoutCancellationToken(IntInput i, Func<StringInput, Task<MultiInput>> next)
        {
            i.Increment();
            IntValue = i.Value;
            var r = next(new StringInput(i)).GetAwaiter().GetResult();
            return Task.FromResult(r.ToString()!);
        }
    }

    public class Decorators
    {
        public static MethodInfo[] GetMethods()
        {
            return typeof(Decorators).GetMethods()
                .Where(m => m.Name.Contains("Handle"))
                .ToArray();
        }

        public void HandleIntInput(IntInput intInput, Action next)
        {
            intInput.Increment();
            next();
        }

        public string HandleIntInputResultString(IntInput intInput, Func<string> next)
        {
            intInput.Increment();
            return next();
        }

        public void HandleStringInput(StringInput intInput, Action next)
        {
            intInput.Increment();
            next();
        }

        public string HandleStringInputResultString(StringInput intInput, Func<string> next)
        {
            intInput.Increment();
            return next();
        }

        public MultiInput HandleStringInputResultMultiInput(StringInput intInput, Func<MultiInput> next)
        {
            intInput.Increment();
            var result = next();
            result.Increment();
            return result;
        }
    }
}
