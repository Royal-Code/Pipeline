using Microsoft.Extensions.DependencyInjection;
using RoyalCode.CommandAndQuery;
using RoyalCode.PipelineFlow.Benchmarks.CommandAndQueryModels;
using System.Diagnostics;

namespace RoyalCode.PipelineFlow.Benchmarks
{
    internal static class PipelineFlowTests
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddCommandsAndQueriesFromAssemblyOfType<Program>();
            services.AddScoped<SingleService>();
            services.AddScoped<SingleWithResultService>();
            services.AddScoped<DecoratedService>();
        }

        public static async Task<string> SingleRequestTest(bool warmup, IServiceProvider sp)
        {
            var stopwatch = new Stopwatch();
            int count = warmup ? Consts.WarmUpCount : Consts.TestCount;

            
            for (int i = 0; i < count; i++)
            {
                using var scope = sp.CreateScope();
                var services = scope.ServiceProvider;

                stopwatch.Start();
                var bus = services.GetService<ICommandQueryBus>()!;

                await bus.SendAsync(new SingleRequest(1));
                stopwatch.Stop();

                if (services.GetService<SingleService>()!.ReceivedValue != 1)
                    throw new Exception("ReceivedValue not setted");
            }
            

            return $"PipelineFlowTests -> SingleRequestTest -> {stopwatch.Elapsed}";
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
                var bus = services.GetService<ICommandQueryBus>()!;

                var result = await bus.SendAsync(new SingleWithResultRequest(1));
                stopwatch.Stop();

                if (result != "1")
                    throw new Exception("Result is wrong");

                if (services.GetService<SingleWithResultService>()!.ReceivedValue != 1)
                    throw new Exception("ReceivedValue not setted");
            }


            return $"PipelineFlowTests -> SingleWithResultRequestTest -> {stopwatch.Elapsed}";
        }

        public static async Task<string> DecoratedRequestTest(bool warmup, IServiceProvider sp)
        {
            var stopwatch = new Stopwatch();
            int count = warmup ? Consts.WarmUpCount : Consts.TestCount;


            for (int i = 0; i < count; i++)
            {
                using var scope = sp.CreateScope();
                var services = scope.ServiceProvider;

                stopwatch.Start();
                var bus = services.GetService<ICommandQueryBus>()!;

                await bus.SendAsync(new DecoratedRequest(1));
                stopwatch.Stop();

                var ds = services.GetService<DecoratedService>()!;
                if (ds.ReceivedValue != 3)
                    throw new Exception($"ReceivedValue is wrong, the current value is {ds.ReceivedValue}");
            }


            return $"PipelineFlowTests -> DecoretedRequestTest -> {stopwatch.Elapsed}";
        }
    }
}
