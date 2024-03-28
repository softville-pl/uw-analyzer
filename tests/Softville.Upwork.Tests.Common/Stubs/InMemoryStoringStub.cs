// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Text;
using Softville.Upwork.BusinessLogic.Processor.Storing;
using Softville.Upwork.Contracts;
using Softville.Upwork.Tests.Common.Data;

namespace Softville.Upwork.Tests.Common.Stubs;

public class InMemoryStoringStub : IHttpResponseStoring
{
    private readonly ConcurrentDictionary<(IUpworkRequestType, UpworkId), string> _cache = new();

    public string? GetDetails(UpworkId id) => GetImpl(DetailsRequest.Instance, id);
    public string? GetApplicants(UpworkId id) => GetImpl(ApplicantsRequest.Instance, id);
    public string? GetSearchResult(UpworkId id) => GetImpl(SearchRequest.Instance, id);

    private string? GetImpl(IUpworkRequestType requestType, UpworkId id)
    {
        _cache.TryGetValue((requestType, id), out string? result);

        return result;
    }

    public void AddDetails(UpworkId id, string content) => AddImpl(id, DetailsRequest.Instance, content);

    public async Task AddDetailsAsync(ResourceOffer offer) =>
        AddImpl(offer.Id, DetailsRequest.Instance, await offer.GetText());

    public void AddApplicants(UpworkId id, string content) => AddImpl(id, ApplicantsRequest.Instance, content);

    public async Task AddApplicantsAsync(ResourceOffer offer) =>
        AddImpl(offer.Id, ApplicantsRequest.Instance, await offer.GetText());

    public async Task<HttpResponseMessage> PersistAsync(UpworkId id, IUpworkRequestType requestType,
        HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode is false)
            return response;

        string content = await response.Content.ReadAsStringAsync(ct);

        AddImpl(id, requestType, content);

        return response;
    }

    public ValueTask PersistJsonAsync(UpworkId id, IUpworkRequestType requestType, string jsonString,
        CancellationToken ct)
    {
        AddImpl(id, requestType, jsonString);
        return ValueTask.CompletedTask;
    }

    public Task<string?> ReadAsync(UpworkId id, IUpworkRequestType requestType, CancellationToken ct)
    {
        _cache.TryGetValue((requestType, id), out string? result);

        return Task.FromResult(result);
    }

    public ValueTask<UpworkId[]> ListAllAsync(CancellationToken ct) =>
        ValueTask.FromResult(_cache.Keys.Select(k => k.Item2).ToArray());

    public Stream? Read(UpworkId id, IUpworkRequestType requestType)
    {
        _cache.TryGetValue((requestType, id), out string? result);

        return result is not null ? new MemoryStream(Encoding.UTF8.GetBytes(result)) : null;
    }

    private void AddImpl(UpworkId id, IUpworkRequestType requestType, string content)
    {
        var cacheId = (requestType, id);

        if (_cache.TryAdd(cacheId, content) is false)
            _cache.TryUpdate(cacheId, content, _cache[cacheId]);
    }
}
