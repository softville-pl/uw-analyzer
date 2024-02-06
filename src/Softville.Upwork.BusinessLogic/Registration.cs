// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Softville.Upwork.BusinessLogic.Processor;

namespace Softville.Upwork.BusinessLogic;

public static class Registration
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services) =>
        services.AddScoped<IUpworkProcessor, UpworkProcessor>()
        .AddScoped<IUpworkProvider, UpworkProvider>();
}
