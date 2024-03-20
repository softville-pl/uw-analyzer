// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Softville.Upwork.BusinessLogic.Common.Configuration;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Common.Database;

internal static class MongoRegistration
{
    static MongoRegistration()
    {
        BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local, BsonType.Document));

        BsonClassMap.RegisterClassMap<Offer>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapIdMember(o => o.Uid);
        });
    }

    internal static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            string connectionString = sp.GetRequiredService<IOptions<DbConfig>>().Value.ConnectionString;

            return new MongoClient(connectionString);
        });

        services.AddScoped<IMongoDatabase>(sp =>
            sp.GetRequiredService<MongoClient>().GetDatabase(
                sp.GetRequiredService<IOptions<DbConfig>>().Value.DatabaseName));

        return services;
    }
}
