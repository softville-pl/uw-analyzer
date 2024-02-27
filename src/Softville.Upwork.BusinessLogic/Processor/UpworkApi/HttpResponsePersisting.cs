// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.BusinessLogic.Common.Extensions;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.UpworkApi;

internal class LocalDiskPersisting : IHttpResponsePersisting
{
    private const string LocalDir =
        @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.BusinessLogic\Processor\Data";

    public async Task<HttpResponseMessage> PersistAsync(UpworkId id, IUpworkRequestType requestType,
        HttpResponseMessage response,
        CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            string textHttpContent = await response.Content.ReadAsStringAsync(ct);

            string outputPath = Path.Combine(LocalDir, $"{id.Uid}-{requestType.RequestName}.json");

            await File.WriteAllTextAsync(outputPath, textHttpContent.JsonPrettify(), ct);
        }

        return response;
    }
}
