// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using Softville.Upwork.BusinessLogic.Processor.UpworkApi;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.Tests.Common.Stubs;

public class InMemoryPersistingStub : IHttpResponsePersisting
{
    private readonly ConcurrentDictionary<(IUpworkRequestType, UpworkId), string> _cache = new();

    public string? GetDetails(UpworkId id) => GetImpl(DetailsRequest.Instance, id);
    public string? GetApplicants(UpworkId id) => GetImpl(ApplicantsRequest.Instance, id);

    private string? GetImpl(IUpworkRequestType requestType, UpworkId id)
    {
        _cache.TryGetValue((requestType, id), out string? result);

        return result;
    }

    public void AddDetails(UpworkId id, string content) => AddImpl(id, DetailsRequest.Instance, content);
    public void AddApplicants(UpworkId id, string content) => AddImpl(id, ApplicantsRequest.Instance, content);

    public async Task<HttpResponseMessage> PersistAsync(UpworkId id, IUpworkRequestType requestType,
        HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode is false)
            return response;

        string content = await response.Content.ReadAsStringAsync(ct);

        AddImpl(id, requestType, content);

        return response;
    }

    private void AddImpl(UpworkId id, IUpworkRequestType requestType, string content)
    {
        var cacheId = (requestType, id);

        if (_cache.TryAdd(cacheId, content) is false)
            _cache.TryUpdate(cacheId, content, _cache[cacheId]);
    }
}
