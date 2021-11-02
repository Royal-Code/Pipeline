
using Microsoft.Extensions.DependencyInjection;
using RoyalCode.PipelineFlow.Benchmarks;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

PipelineFlowTests.AddServices(services);
MediatRTests.AddServices(services);

var sp = services.BuildServiceProvider();

Console.WriteLine("Warmup");

var result = await PipelineFlowTests.SingleRequestTest(true, sp);
Console.WriteLine(result);
result = await MediatRTests.SingleRequestTest(true, sp);
Console.WriteLine(result);

result = await PipelineFlowTests.SingleWithResultRequestTest(true, sp);
Console.WriteLine(result);
result = await MediatRTests.SingleWithResultRequestTest(true, sp);
Console.WriteLine(result);

result = await PipelineFlowTests.DecoratedRequestTest(true, sp);
Console.WriteLine(result);
result = await MediatRTests.DecoratedRequestTest(true, sp);
Console.WriteLine(result);


Console.WriteLine("Testing");

result = await PipelineFlowTests.SingleRequestTest(false, sp);
Console.WriteLine(result);
result = await MediatRTests.SingleRequestTest(false, sp);
Console.WriteLine(result);

result = await PipelineFlowTests.SingleWithResultRequestTest(false, sp);
Console.WriteLine(result);
result = await MediatRTests.SingleWithResultRequestTest(false, sp);
Console.WriteLine(result);

result = await PipelineFlowTests.DecoratedRequestTest(false, sp);
Console.WriteLine(result);
result = await MediatRTests.DecoratedRequestTest(false, sp);
Console.WriteLine(result);

Console.ReadKey();
