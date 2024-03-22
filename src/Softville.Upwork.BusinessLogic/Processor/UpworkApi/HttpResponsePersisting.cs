// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Common.Extensions;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.UpworkApi;

internal class LocalDiskStoring(ILogger<LocalDiskStoring> logger) : IHttpResponseStoring
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

            string outputPath = Path.Combine(LocalDir,
                $"{id.Uid}{GetPostfix(requestType)}.json");

            await File.WriteAllTextAsync(outputPath, textHttpContent.JsonPrettify(), ct);
        }

        return response;
    }

    public async Task<string?> ReadAsync(UpworkId id, IUpworkRequestType requestType,
        CancellationToken ct)
    {
        string inputPath = Path.Combine(LocalDir, $"{id.Uid}{GetPostfix(requestType)}.json");

        string? result = null;

        if (Path.Exists(inputPath))
        {
            result = await File.ReadAllTextAsync(inputPath, ct);
        }

        return result;
    }

    public Stream? Read(UpworkId id, IUpworkRequestType requestType)
    {
        string inputPath = Path.Combine(LocalDir, $"{id.Uid}{GetPostfix(requestType)}.json");

        Stream? result = null;

        if (Path.Exists(inputPath))
            result = File.OpenRead(inputPath);

        return result;
    }

    static readonly Regex _regex = new ("\"ciphertext\":\\s*\"([^\"]*)\"", RegexOptions.Compiled);

    public async ValueTask<UpworkId[]> ListAllAsync(CancellationToken ct)
    {
        var offerDetailFiles = Directory.GetFiles(LocalDir, "???????????????????.json");

        ConcurrentBag<UpworkId> result = new();

        await Parallel.ForEachAsync(offerDetailFiles, ct, async (filePath, ct1) =>
        {
            var match = _regex.Match(await File.ReadAllTextAsync(filePath, ct1));
            string uid = Path.GetFileName(filePath).Substring(0, 19);

            if (match.Success)
            {
                result.Add(new UpworkId(uid, '~' + match.Value));
            }
            else
            {
                logger.LogWarning("{uid} offer's cipherText cannot be obtained", uid);
            }
        });

        return result.ToArray();
    }

    private static string GetPostfix(IUpworkRequestType requestType) =>
        requestType.RequestName.Length > 0 ? $"-{requestType.RequestName}" : "";
}
