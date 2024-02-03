// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor;

internal record struct OfferSerializer
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true, WriteIndented = true
    };

    [RequiresUnreferencedCode(
        "Calls System.Text.Json.JsonSerializer.SerializeAsync<TValue>(Stream, TValue, JsonSerializerOptions, CancellationToken)")]
    [RequiresDynamicCode(
        "Calls System.Text.Json.JsonSerializer.SerializeAsync<TValue>(Stream, TValue, JsonSerializerOptions, CancellationToken)")]
    internal async Task<Stream> SerializeAsync(Offer offer, CancellationToken ct)
    {
        MemoryStream ms = new();
        await JsonSerializer.SerializeAsync(ms, offer, _options, ct);

        return ms;
    }
}
