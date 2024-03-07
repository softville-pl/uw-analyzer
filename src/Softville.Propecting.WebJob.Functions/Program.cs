using Microsoft.Extensions.Hosting;
using Prospecting.WebJob.Common;

var host = WebJobHostBuilder.CreateBuilder(Array.Empty<string>())
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
