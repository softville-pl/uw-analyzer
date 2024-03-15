// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Softville.Upwork.BusinessLogic.Common.Configuration;

namespace Softville.Upwork.BusinessLogic.Common.Database;

internal static class MongoRegistration
{
    internal static IServiceCollection AddMongoClient(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            string connectionString = sp.GetRequiredService<IOptions<DbConfig>>().Value.ConnectionString;

            return new MongoClient(connectionString);
        });

        return services;
    }
}
