using Microsoft.Extensions.Hosting;
using Prospecting.WebJob.Common;

var host = WebJobHostBuilder.CreateBuilder(Array.Empty<string>())
    .ConfigureFunctionsWorkerDefaults((context, builder) => { }, options => { } )
    .Build();

host.Run();
