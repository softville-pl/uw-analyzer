﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Softville.Upwork.BusinessLogic.Processor;

internal record struct UpworkParser
{
    private static readonly JsonSerializerOptions? _options = new() {PropertyNameCaseInsensitive = true};

    [RequiresDynamicCode(
        "Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    [RequiresUnreferencedCode(
        "Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    public async ValueTask<UpworkOffer> ParseAsync(Stream utf8Json, CancellationToken ct) =>
        await JsonSerializer.DeserializeAsync<UpworkOffer>(utf8Json, _options, ct) ??
        throw new ArgumentNullException(nameof(utf8Json), "Stream parsed to null offer");
}
