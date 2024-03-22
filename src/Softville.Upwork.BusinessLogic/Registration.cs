// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Softville.Upwork.BusinessLogic.Common.Configuration;
using Softville.Upwork.BusinessLogic.Common.Database;
using Softville.Upwork.BusinessLogic.Processor;
using Softville.Upwork.BusinessLogic.Processor.ApplicantsStats;
using Softville.Upwork.BusinessLogic.Processor.Migrator;
using Softville.Upwork.BusinessLogic.Processor.OfferDetails;
using Softville.Upwork.BusinessLogic.Processor.Repositories;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;
using Softville.Upwork.BusinessLogic.Queries.Offers;

namespace Softville.Upwork.BusinessLogic;

public static class Registration
{
    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functioconfig = {ConfigurationRoot} Sections = 65 nality when trimming application code",
        Justification = "<Pending>")]
    [UnconditionalSuppressMessage("AOT",
        "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
        Justification = "<Pending>")]
    public static IServiceCollection AddProcessorWebJob(this IServiceCollection services, IConfiguration config)
    {
        services
            .Configure<WebJobConfig>(config.GetSection(WebJobConfig.Name))
            .Configure<UpworkConfig>(config.GetSection($"{WebJobConfig.Name}:{UpworkConfig.Name}"))
            .Configure<DbConfig>(config.GetSection($"{WebJobConfig.Name}:{DbConfig.Name}"))
            .AddHttpClient(UpworkHttpClient.UpworkClientName, UpworkHttpClient.ConfigureDetailsClient)
            .ConfigurePrimaryHttpMessageHandler(_ =>
                new HttpClientHandler {AutomaticDecompression = DecompressionMethods.All});

        return services
            .AddScoped<IUpworkProcessor, EndToEndUpworkProcessor>()
            .AddScoped<ISearchResultProvider, SearchResultProvider>()
            .AddScoped<IUpworkApiCaller, UpworkApiCaller>()
            .AddScoped<IHttpResponseStoring, LocalDiskStoring>()
            .AddScoped<IApplicantsClient, ApplicantsUpworkClient>()
            .AddScoped<IApplicantsStatsProvider, ApplicantsStatsProvider>()
            .AddScoped<IOfferDetailsProvider, OfferDetailsProvider>()
            .AddScoped<IOfferDetailsUpworkClient, OfferDetailsUpworkClient>()
            .AddScoped<IOfferProcessorRepository, OfferProcessorRepository>()
            .AddScoped<IOffersMigrator, OffersMigrator>()
            .AddMongo()
            ;

    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functioconfig = {ConfigurationRoot} Sections = 65 nality when trimming application code",
        Justification = "<Pending>")]
    [UnconditionalSuppressMessage("AOT",
        "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
        Justification = "<Pending>")]
    public static IServiceCollection AddProspectingWebApi(this IServiceCollection services, IConfiguration config)
    {
        services
            .Configure<WebApiConfig>(config.GetSection(WebApiConfig.Name))
            .Configure<DbConfig>(config.GetSection($"{WebApiConfig.Name}:{DbConfig.Name}"));

        return services
            .AddScoped<IOfferProcessorRepository, OfferProcessorRepository>()
            .AddScoped<IGetOffersQuery, GetOffersQuery>()
            .AddMongo()
            ;

    }
}
