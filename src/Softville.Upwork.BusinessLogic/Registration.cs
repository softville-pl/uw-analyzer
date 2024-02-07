// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Softville.Upwork.BusinessLogic.Processor;
using Softville.Upwork.BusinessLogic.Processor.Configuration;

namespace Softville.Upwork.BusinessLogic;

public static class Registration
{
    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functioconfig = {ConfigurationRoot} Sections = 65 nality when trimming application code",
        Justification = "<Pending>")]
    [UnconditionalSuppressMessage("AOT",
        "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
        Justification = "<Pending>")]
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services, IConfiguration config)
    {
        services
            .Configure<UpworkConfig>(config)
            .AddHttpClient(UpworkHttpClient.UpworkClientName, UpworkHttpClient.ConfigureDetailsClient)
            .ConfigurePrimaryHttpMessageHandler(_ =>
                new HttpClientHandler {AutomaticDecompression = DecompressionMethods.All});

        return services
            .AddScoped<IUpworkProcessor, EndToEndUpworkProcessor>()
            .AddScoped<ISearchResultProvider, SearchResultProvider>();
    }
}
