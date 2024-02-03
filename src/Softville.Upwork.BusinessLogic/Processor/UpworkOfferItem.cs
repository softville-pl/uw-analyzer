// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Softville.Upwork.BusinessLogic.Processor;

public record UpworkOfferItem
{
    public required string Id { get; set; }
    public required string Ciphertext { get; set; }
    public DateTime PublishedDateTime { get; set; }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
        Justification = "<Pending>")]
    [UnconditionalSuppressMessage("AOT",
        "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
        Justification = "<Pending>")]
    internal static UpworkOfferItem[] OffersToProcess => JsonSerializer.Deserialize<UpworkOfferItem[]>(
        File.ReadAllText(
            @"d:\Sources\Softville\uw-analyzer\src\Softville.Upwork.BusinessLogic\Processor\Data\new-offers.json"),
        new JsonSerializerOptions {PropertyNameCaseInsensitive = true})!;
}
