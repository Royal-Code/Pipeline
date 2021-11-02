using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RoyalCode.PipelineFlow.Benchmarks.MediatRModels;
using System.Diagnostics;

namespace RoyalCode.PipelineFlow.Benchmarks
{
    internal static class MediatRTests
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Program));
            services.AddScoped<SingleService>();
            services.AddScoped<SingleWithResultService>();
            services.AddScoped<DecoratedService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DecoratorHandlerOne<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DecoratorHandlerTwo<,>));
        }

        public static async Task<string> SingleRequestTest(bool warmup, IServiceProvider sp)
        {
            var stopwatch = new Stopwatch();
            int count = warmup ? 2_000 : 100_000;

            
            for (int i = 0; i < count; i++)
            {
                using var scope = sp.CreateScope();
                var services = scope.ServiceProvider;

                stopwatch.Start();
                var mediator = services.GetService<IMediator>()!;

                await mediator.Send(new SingleRequest(1));
                stopwatch.Stop();

                if (services.GetService<SingleService>()!.ReceivedValue != 1)
                    throw new Exception("ReceivedValue not setted");
            }
            

            return $"MediatRTests -> SingleRequestTest -> {stopwatch.Elapsed}";
        }

        public static async Task<string> SingleWithResultRequestTest(bool warmup, IServiceProvider sp)
        {
            var stopwatch = new Stopwatch();
            int count = warmup ? Consts.WarmUpCount : Consts.TestCount;

            for (int i = 0; i < count; i++)
            {
                using var scope = sp.CreateScope();
                var services = scope.ServiceProvider;

                stopwatch.Start();
                var bus = services.GetService<IMediator>()!;

                var result = await bus.Send(new SingleWithResultRequest(1));
                stopwatch.Stop();

                if (result != "1")
                    throw new Exception("Result is wrong");

                if (services.GetService<SingleWithResultService>()!.ReceivedValue != 1)
                    throw new Exception("ReceivedValue not setted");
            }


            return $"MediatRTests -> SingleWithResultRequestTest -> {stopwatch.Elapsed}";
        }

        public static async Task<string> DecoratedRequestTest(bool warmup, IServiceProvider sp)
        {
            var stopwatch = new Stopwatch();
            int count = warmup ? 2_000 : 100_000;


            for (int i = 0; i < count; i++)
            {
                using var scope = sp.CreateScope();
                var services = scope.ServiceProvider;

                stopwatch.Start();
                var mediator = services.GetService<IMediator>()!;

                await mediator.Send(new DecoratedRequest(1));
                stopwatch.Stop();

                var ds = services.GetService<DecoratedService>()!;
                if (ds.ReceivedValue != 3)
                    throw new Exception($"ReceivedValue is wrong, the current value is {ds.ReceivedValue}");
            }


            return $"MediatRTests -> DecoratedRequestTest -> {stopwatch.Elapsed}";
        }
    }
}
