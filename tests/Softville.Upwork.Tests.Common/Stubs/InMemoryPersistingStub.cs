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

    public async Task<HttpResponseMessage> PersistAsync(UpworkId id, IUpworkRequestType requestType,
        HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode is false)
            return response;

        string content = await response.Content.ReadAsStringAsync(ct);

        if (_cache.TryAdd((requestType, id), content) is false)
            throw new InvalidOperationException($"{id} {requestType.GetType().Name} already added");

        return response;
    }
}
