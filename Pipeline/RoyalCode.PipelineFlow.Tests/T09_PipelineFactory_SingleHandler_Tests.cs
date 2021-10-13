using Xunit;
using RoyalCode.PipelineFlow.Configurations;
using System.Threading.Tasks;
using System.Threading;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T09_PipelineFactory_SingleHandler_Tests
    {
        [Fact]
        public  void T01_SingleInputHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.AddHandlerMethodDefined(typeof(SingleInputHandler), "Handle");
                })
                .Create();

            var pipeline = factory.Create<SingleInput>();
            Assert.NotNull(pipeline);

            pipeline.Send(new SingleInput(1));
            Assert.Equal(1, SingleInputHandler.Value);
        }

        [Fact]
        public void T02_SingleInputOutputHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.AddHandlerMethodDefined(typeof(SingleInputOutputHandler), "Handle");
                })
                .Create();

            var pipeline = factory.Create<SingleInputOutput, int>();
            Assert.NotNull(pipeline);

            var output = pipeline.Send(new SingleInputOutput(1));
            Assert.Equal(1, output);
            Assert.Equal(1, SingleInputOutputHandler.Value);
        }

        [Fact]
        public void T03_SingleAsyncInputHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.AddHandlerMethodDefined(typeof(SingleAsyncInputHandler), "Handle");
                })
                .Create();

            var pipeline = factory.Create<SingleAsyncInput>();
            Assert.NotNull(pipeline);

            pipeline.SendAsync(new SingleAsyncInput(1)).GetAwaiter().GetResult();
            Assert.Equal(1, SingleAsyncInputHandler.Value);
        }

        [Fact]
        public void T04_SingleAsyncInputWithoutCancellationTokenHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.AddHandlerMethodDefined(typeof(SingleAsyncInputWithoutCancellationTokenHandler), "Handle");
                })
                .Create();

            var pipeline = factory.Create<SingleAsyncInput>();
            Assert.NotNull(pipeline);

            pipeline.SendAsync(new SingleAsyncInput(1)).GetAwaiter().GetResult();
            Assert.Equal(1, SingleAsyncInputWithoutCancellationTokenHandler.Value);
        }

        [Fact]
        public void T05_SingleInputOutputAsyncHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.AddHandlerMethodDefined(typeof(SingleInputOutputAsyncHandler), "Handle");
                })
                .Create();

            var pipeline = factory.Create<SingleInputOutputAsync, int>();
            Assert.NotNull(pipeline);

            var output = pipeline.SendAsync(new SingleInputOutputAsync(1)).GetAwaiter().GetResult();
            Assert.Equal(1, output);
            Assert.Equal(1, SingleInputOutputAsyncHandler.Value);
        }

        [Fact]
        public void T06_SingleInputOutputAsyncWithoutCancellationTokenHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.AddHandlerMethodDefined(typeof(SingleInputOutputAsyncWithoutCancellationTokenHandler), "Handle");
                })
                .Create();

            var pipeline = factory.Create<SingleInputOutputAsync, int>();
            Assert.NotNull(pipeline);

            var output = pipeline.SendAsync(new SingleInputOutputAsync(1)).GetAwaiter().GetResult();
            Assert.Equal(1, output);
            Assert.Equal(1, SingleInputOutputAsyncHandler.Value);
        }

        [Fact]
        public void T07_SingleInputDelegateHandler()
        {
            int result = 0;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.Configure<SingleInput>()
                        .Handle(input => result = input.Value);
                })
                .Create();

            var pipeline = factory.Create<SingleInput>();
            Assert.NotNull(pipeline);

            pipeline.Send(new SingleInput(1));
            Assert.Equal(1, result);
        }

        [Fact]
        public void T08_SingleInputOutputDelegateHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.Configure<SingleInputOutput, int>()
                        .Handle(input => input.Value + 1);
                })
                .Create();

            var pipeline = factory.Create<SingleInputOutput, int>();
            Assert.NotNull(pipeline);

            var output = pipeline.Send(new SingleInputOutput(1));
            Assert.Equal(2, output);
        }

        [Fact]
        public void T09_SingleAsyncInputDelegateHandler()
        {
            int result = 0;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.Configure<SingleAsyncInput>()
                        .HandleAsync((input, token) => { result = input.Value; return Task.CompletedTask; });
                })
                .Create();

            var pipeline = factory.Create<SingleAsyncInput>();
            Assert.NotNull(pipeline);

            pipeline.SendAsync(new SingleAsyncInput(1)).GetAwaiter().GetResult();
            Assert.Equal(1, result);
        }

        [Fact]
        public void T10_SingleAsyncInputDelegateWithoutCancellationTokenHandler()
        {
            int result = 0;

            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.Configure<SingleAsyncInput>()
                        .HandleAsync(input => { result = input.Value; return Task.CompletedTask; });
                })
                .Create();

            var pipeline = factory.Create<SingleAsyncInput>();
            Assert.NotNull(pipeline);

            pipeline.SendAsync(new SingleAsyncInput(1)).GetAwaiter().GetResult();
            Assert.Equal(1, result);
        }

        [Fact]
        public void T11_SingleInputOutputAsyncDelegateHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.Configure<SingleInputOutputAsync, int>()
                        .HandleAsync((input, token) => Task.FromResult(input.Value));
                })
                .Create();

            var pipeline = factory.Create<SingleInputOutputAsync, int>();
            Assert.NotNull(pipeline);

            var output = pipeline.SendAsync(new SingleInputOutputAsync(1)).GetAwaiter().GetResult();
            Assert.Equal(1, output);
        }

        [Fact]
        public void T12_SingleInputOutputAsyncWithoutCancellationTokenDelegateHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.Configure<SingleInputOutputAsync, int>()
                        .HandleAsync(input => Task.FromResult(input.Value));
                })
                .Create();

            var pipeline = factory.Create<SingleInputOutputAsync, int>();
            Assert.NotNull(pipeline);

            var output = pipeline.SendAsync(new SingleInputOutputAsync(1)).GetAwaiter().GetResult();
            Assert.Equal(1, output);
        }
    }

    internal interface ITestBus { }

    #region T01 & 07
    public class SingleInput
    {
        public int Value { get; }

        public SingleInput(int value)
        {
            Value = value;
        }
    }

    public class SingleInputHandler
    {
        public static int Value = 0;

        public void Handle(SingleInput input)
        {
            Value = input.Value;
        }
    }
    #endregion

    #region T02 & 08
    public class SingleInputOutput
    {
        public int Value { get; }

        public SingleInputOutput(int value)
        {
            Value = value;
        }
    }

    public class SingleInputOutputHandler
    {
        public static int Value = 0;

        public int Handle(SingleInputOutput input)
        {
            Value = input.Value;
            return Value;
        }
    }
    #endregion

    #region T03 & 09
    public class SingleAsyncInput
    {
        public int Value { get; }

        public SingleAsyncInput(int value)
        {
            Value = value;
        }
    }

    public class SingleAsyncInputHandler
    {
        public static int Value = 0;

        public Task Handle(SingleAsyncInput input, CancellationToken cancellationToken)
        {
            Value = input.Value;
            return Task.CompletedTask;
        }
    }
    #endregion

    #region T04 & 10

    public class SingleAsyncInputWithoutCancellationTokenHandler
    {
        public static int Value = 0;

        public Task Handle(SingleAsyncInput input)
        {
            Value = input.Value;
            return Task.CompletedTask;
        }
    }
    #endregion

    #region T05 & 11
    public class SingleInputOutputAsync
    {
        public int Value { get; }

        public SingleInputOutputAsync(int value)
        {
            Value = value;
        }
    }

    public class SingleInputOutputAsyncHandler
    {
        public static int Value = 0;

        public Task<int> Handle(SingleInputOutputAsync input, CancellationToken cancellationToken)
        {
            Value = input.Value;
            return Task.FromResult(Value);
        }
    }
    #endregion

    #region T06 & 12
    public class SingleInputOutputAsyncWithoutCancellationTokenHandler
    {
        public static int Value = 0;

        public Task<int> Handle(SingleInputOutputAsync input)
        {
            Value = input.Value;
            return Task.FromResult(Value);
        }
    }
    #endregion
}
