using Microsoft.Extensions.DependencyInjection;
using RoyalCode.CommandAndQuery.Tests.GenericsDecorators;
using RoyalCode.PipelineFlow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.CommandAndQuery.Tests
{
    public class T05_FullPipelineTests
    {
        // Ter um comando de entrada,
        // Um bridge handler para um segundo comando.
        // um decorador genérico, e um decorador específico
        // o decorador genérico terá que ser executado sobre os dois comandos,
        // deve haver dois decoradores específicos, para cada comando.

        [Fact]
        public async Task T01_FullPipeline_In()
        {
            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<GenericDecoratorIn<FullPipelineInRequest1>>();
            services.AddCommandsAndQueriesFromAssemblyOfType<FullPipelineInRequest1>();
            services.AddSingleton<Action<FullPipelineInRequest1>>(request => request.Items.Add(nameof(GenericDecoratorIn<FullPipelineInRequest1>) + $"<{nameof(FullPipelineInRequest1)}>"));
            services.AddSingleton<Action<FullPipelineInRequest2>>(request => request.Items.Add(nameof(GenericDecoratorIn<FullPipelineInRequest2>) + $"<{nameof(FullPipelineInRequest2)}>"));

            var expected = new List<string>(6)
            {
                nameof(GenericDecoratorIn<FullPipelineInRequest1>) + $"<{nameof(FullPipelineInRequest1)}>",
                nameof(FullPipelineInDecorator1),
                nameof(FullPipelineInBridge),
                nameof(GenericDecoratorIn<FullPipelineInRequest2>) + $"<{nameof(FullPipelineInRequest2)}>",
                nameof(FullPipelineInDecorator2),
                nameof(FullPipelineInHandle)
            };

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new FullPipelineInRequest1();
            bus.Send(request);
            Assert.Equal(expected.Count, request.Items.Count);
            Assert.Equal(expected, request.Items);

            request = new FullPipelineInRequest1();
            await bus.SendAsync(request);
            Assert.Equal(expected.Count, request.Items.Count);
            Assert.Equal(expected, request.Items);
        }

        [Fact]
        public async Task T02_FullPipeline_InOut()
        {
            PipelineFactory.ResetChainTypes<ICommandQueryBus>();
            var services = new ServiceCollection();

            services.AddCommandsAndQueriesFromAssemblyOfType<GenericDecoratorInOut<FullPipelineInOutRequest1, PipelineItems>>();
            services.AddCommandsAndQueriesFromAssemblyOfType<FullPipelineInOutRequest1>();
            services.AddSingleton<Action<FullPipelineInOutRequest1>>(request => request.Items.Add(nameof(GenericDecoratorInOut<FullPipelineInOutRequest1, PipelineItems>) + $"<{nameof(FullPipelineInRequest1)}, {nameof(PipelineItems)}>"));
            services.AddSingleton<Action<FullPipelineInOutRequest2>>(request => request.Items.Add(nameof(GenericDecoratorInOut<FullPipelineInOutRequest2, PipelineItems>) + $"<{nameof(FullPipelineInRequest2)}, {nameof(PipelineItems)}>"));
            services.AddSingleton<Action<PipelineItems>>(items => items.AddName<PipelineItems>());

            var expectedIn = new List<string>(6)
            {
                nameof(GenericDecoratorInOut<FullPipelineInOutRequest1, PipelineItems>) + $"<{nameof(FullPipelineInRequest1)}, {nameof(PipelineItems)}>",
                nameof(FullPipelineInOutDecorator1),
                nameof(FullPipelineInOutBridge),
                nameof(GenericDecoratorInOut<FullPipelineInOutRequest2, PipelineItems>) + $"<{nameof(FullPipelineInRequest2)}, {nameof(PipelineItems)}>",
                nameof(FullPipelineInOutDecorator2),
                nameof(FullPipelineInOutHandle)
            };

            var expectedOut = new List<string>(6)
            {
                nameof(FullPipelineInOutHandle),
                nameof(FullPipelineInOutDecorator2),
                nameof(PipelineItems),
                nameof(FullPipelineInOutBridge),
                nameof(FullPipelineInOutDecorator1),
                nameof(PipelineItems),
            };

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);

            var request = new FullPipelineInOutRequest1();
            var output = bus.Send(request);
            Assert.Equal(expectedIn.Count, request.Items.Count);
            Assert.Equal(expectedIn, request.Items);
            Assert.Equal(expectedOut.Count, output.Count);
            Assert.Equal(expectedOut, output);

            request = new FullPipelineInOutRequest1();
            output = await bus.SendAsync(request);
            Assert.Equal(expectedIn.Count, request.Items.Count);
            Assert.Equal(expectedIn, request.Items);
            Assert.Equal(expectedOut.Count, output.Count);
            Assert.Equal(expectedOut, output);
        }
    }

    public class PipelineItems : List<string> 
    {
        public void AddName<T>() => Add(typeof(T).Name);
    }

    #region In

    public class FullPipelineInRequest1 : IRequest
    {
        public PipelineItems Items { get; } = new PipelineItems();
    }

    public class FullPipelineInRequest2 : IRequest
    {
        public PipelineItems Items { get; }

        public FullPipelineInRequest2(PipelineItems items)
        {
            Items = items;
        }
    }

    public class FullPipelineInDecorator1 : IDecorator<FullPipelineInRequest1>
    {
        public void Handle(FullPipelineInRequest1 request, Action next)
        {
            request.Items.AddName<FullPipelineInDecorator1>();
            next();
        }
    }

    public class FullPipelineInBridge : IBridge<FullPipelineInRequest1, FullPipelineInRequest2>
    {
        public void Next(FullPipelineInRequest1 request, Action<FullPipelineInRequest2> next)
        {
            request.Items.AddName<FullPipelineInBridge>();
            next(new FullPipelineInRequest2(request.Items));
        }
    }

    public class FullPipelineInDecorator2 : IDecorator<FullPipelineInRequest2>
    {
        public void Handle(FullPipelineInRequest2 request, Action next)
        {
            request.Items.AddName<FullPipelineInDecorator2>();
            next();
        }
    }

    public class FullPipelineInHandle : IHandler<FullPipelineInRequest2>
    {
        public void Handle(FullPipelineInRequest2 request)
        {
            request.Items.AddName<FullPipelineInHandle>();
        }
    }

    #endregion

    #region InOut

    public class FullPipelineInOutRequest1 : IRequest<PipelineItems>
    {
        public PipelineItems Items { get; } = new PipelineItems();
    }

    public class FullPipelineInOutRequest2 : IRequest<PipelineItems>
    {
        public PipelineItems Items { get; }

        public FullPipelineInOutRequest2(PipelineItems items)
        {
            Items = items;
        }
    }

    public class FullPipelineInOutDecorator1 : IDecorator<FullPipelineInOutRequest1, PipelineItems>
    {
        public PipelineItems Handle(FullPipelineInOutRequest1 request, Func<PipelineItems> next)
        {
            request.Items.AddName<FullPipelineInOutDecorator1>();
            var output = next();
            output.AddName<FullPipelineInOutDecorator1>();
            return output;
        }
    }

    public class FullPipelineInOutBridge : IBridge<FullPipelineInOutRequest1, PipelineItems, FullPipelineInOutRequest2>
    {
        public PipelineItems Next(FullPipelineInOutRequest1 request, Func<FullPipelineInOutRequest2, PipelineItems> next)
        {
            request.Items.AddName<FullPipelineInOutBridge>();
            var output = next(new FullPipelineInOutRequest2(request.Items));
            output.AddName<FullPipelineInOutBridge>();
            return output;
        }
    }

    public class FullPipelineInOutDecorator2 : IDecorator<FullPipelineInOutRequest2, PipelineItems>
    {
        public PipelineItems Handle(FullPipelineInOutRequest2 request, Func<PipelineItems> next)
        {
            request.Items.AddName<FullPipelineInOutDecorator2>();
            var output = next();
            output.AddName<FullPipelineInOutDecorator2>();
            return output;
        }
    }

    public class FullPipelineInOutHandle : IHandler<FullPipelineInOutRequest2, PipelineItems>
    {
        public PipelineItems Handle(FullPipelineInOutRequest2 request)
        {
            request.Items.AddName<FullPipelineInOutHandle>();
            var output = new PipelineItems();
            output.AddName<FullPipelineInOutHandle>();
            return output;
        }
    }

    #endregion
}
