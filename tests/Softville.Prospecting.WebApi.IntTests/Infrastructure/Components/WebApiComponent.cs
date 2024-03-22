// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Alba;
using Microsoft.Extensions.Configuration;
using Softville.Upwork.BusinessLogic.Common.Configuration;

namespace Softville.Upwork.WebApi.IntTests.Infrastructure.Components;

public class WebApiComponent
{
    public IAlbaHost Alba { get; private set; } = null!;

    public async Task StartAsync(WebApiConfig config)
    {
        Alba = await AlbaHost.For<Program>(hostBuilder =>
        {
            hostBuilder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(
                [
                    new($"{WebApiConfig.Name}:{DbConfig.Name}:{nameof(DbConfig.ConnectionString)}", config.Database.ConnectionString)
                ]);
            });
        });
    }

    public async Task StopAsync(CancellationToken ct)
    {
        await Alba.StopAsync(ct);
    }
}
