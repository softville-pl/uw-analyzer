// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Common.Configuration;

namespace Softville.Upwork.WebApi.Extensions;

internal static class CorsExtensions
{
    public static IServiceCollection AddPrpWebApiCors(this IServiceCollection services, string policyName,
        IConfiguration configuration)
    {
        var webApiConfig = new WebApiConfig();
        configuration.Bind(WebApiConfig.Name, webApiConfig);

        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policyBuilder => policyBuilder.WithOrigins(webApiConfig.Cors.CorsOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        return services;
    }
}
