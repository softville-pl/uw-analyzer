// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Queries.Offers;

namespace Softville.Upwork.WebApi;

public static class Endpoints
{
    internal static void MapProspectingWebApi(this IEndpointRouteBuilder app) =>
        app.MapGet("/offers",
            async (IGetOffersQuery command, CancellationToken ct) => await command.GetOffersAsync(ct));
}
