// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class UpworkProcessor : IUpworkProcessor
{
    [RequiresDynamicCode(
        "Calls Softville.Upwork.BusinessLogic.Processor.UpworkParser.ParseAsync(Stream, CancellationToken)")]
    [RequiresUnreferencedCode(
        "Calls Softville.Upwork.BusinessLogic.Processor.UpworkParser.ParseAsync(Stream, CancellationToken)")]
    public async Task ProcessOffersAsync(CancellationToken ct)
    {
        string inputFolder = @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.BusinessLogic\Processor\Data";
        string outputFolder = @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.WebApp\wwwroot\sample-data";

        string regexPattern = @"^\d{19}\.json$"; // Regex pattern for 19-digit number followed by .json extension

        string[] offerFilePaths = Directory.GetFiles(inputFolder, "*.json")
            .Where(file => Regex.IsMatch(Path.GetFileName(file), regexPattern))
            .ToArray();

        UpworkParser parser = new();
        OfferSerializer serializer = new();

        List<Offer> offers = new(offerFilePaths.Length);

        foreach (string offerFilePath in offerFilePaths)
        {
            await using FileStream inputOfferFileStream = File.OpenRead(offerFilePath);
            UpworkOffer upworkOffer = await parser.ParseAsync(inputOfferFileStream, ct);

            offers.Add(upworkOffer.MapToOffer());
        }

        await using Stream offerStream = await serializer.SerializeAsync(offers, ct);

        await using FileStream outputOfferFileStream =
            new(Path.Join(outputFolder, "offers.json"), FileMode.OpenOrCreate);

        await offerStream.CopyToAsync(outputOfferFileStream, ct);

        await outputOfferFileStream.FlushAsync(ct);
        outputOfferFileStream.Close();
    }
}
