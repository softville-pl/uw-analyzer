// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Softville.Upwork.BusinessLogic.Common.Extensions;
using Softville.Upwork.BusinessLogic.Processor.Parsers;
using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class EndToEndUpworkProcessor(
    ISearchResultProvider searchProvider,
    IHttpClientFactory httpClientFactory,
    ILogger<EndToEndUpworkProcessor> logger)
    : IUpworkProcessor
{
    public async Task ProcessOffersAsync(CancellationToken ct)
    {
        List<JobSearch> foundSearchItems = await searchProvider.SearchAsync(ct);

        string rawDetailsOutputFolder =
            @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.BusinessLogic\Processor\Data";

        using HttpClient detailsClient = httpClientFactory.CreateClient(UpworkHttpClient.UpworkClientName);

        List<Offer> offers = new(foundSearchItems.Count);
        List<string> problematicOffers = new();

        UpworkParser parser = new();
        OfferSerializer serializer = new();

        string inputFolder = @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.BusinessLogic\Processor\Data";
        string outputFolder = @"D:\Sources\Softville\uw-analyzer\src\Softville.Upwork.WebApp\wwwroot\sample-data";

        foreach (JobSearch foundOffer in foundSearchItems)
        {
            string outputPath = Path.Combine(rawDetailsOutputFolder, $"{foundOffer.Id}.json");

            string offerDetailsContent;

            if (Path.Exists(outputPath))
            {
                logger.LogWarning(@"'{offerId}{cipherText}' offer details exists. Skipping.", foundOffer.Ciphertext,
                    foundOffer.Id);

                offerDetailsContent = await File.ReadAllTextAsync(outputPath, ct);
            }
            else
            {
                HttpResponseMessage response =
                    await detailsClient.GetAsync(
                        $"https://www.upwork.com/job-details/jobdetails/api/job/{foundOffer.Ciphertext}/summary", ct);

                string content = await response.Content.ReadAsStringAsync(ct);

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation("{offerId}{cipherText} request succeeded", foundOffer.Id,
                        foundOffer.Ciphertext);

                    await File.WriteAllTextAsync(outputPath, content.JsonPrettify(), ct);

                    offerDetailsContent = content;
                }
                else
                {
                    logger.LogError(
                        "{offerId}{cipherText} request failed with status code:{statusCode}. Response: {content}",
                        foundOffer.Id, foundOffer.Ciphertext, response.StatusCode, content);
                    continue;
                }
            }

            string offerId = foundOffer.Id;
            try
            {
                await using MemoryStream inputOfferFileStream = new(Encoding.UTF8.GetBytes(offerDetailsContent));
                UpworkOffer upworkOffer = await parser.ParseAsync(inputOfferFileStream, ct);

                Offer offer = upworkOffer.MapToOffer();
                offer.Requirements = foundOffer.Attrs.Select(attr => attr.PrettyName).ToArray();
                offer.ConnectPrice = foundOffer.ConnectPrice;
                offers.Add(offer);
            }
            catch (Exception e)
            {
                logger.LogError("'{offerId}' couldn't be processed. Reason {reason}", offerId,
                    e.Message + " -> " + e.InnerException?.Message);
                problematicOffers.Add(offerId);
            }
        }


        string problematicOffersOutput = Path.Combine(inputFolder, "problematic-offers.json");

        string regexPattern = @"^\d{19}\.json$"; // Regex pattern for 19-digit number followed by .json extension

        string[] offerFilePaths = Directory.GetFiles(inputFolder, "*.json")
            .Where(file => Regex.IsMatch(Path.GetFileName(file), regexPattern))
            .ToArray();

        await using Stream offerStream = await serializer.SerializeAsync(offers, ct);

        await using FileStream outputOfferFileStream =
            new(Path.Join(outputFolder, "offers.json"), FileMode.OpenOrCreate);

        await offerStream.CopyToAsync(outputOfferFileStream, ct);

        await outputOfferFileStream.FlushAsync(ct);
        outputOfferFileStream.Close();

        await File.WriteAllTextAsync(problematicOffersOutput, string.Join(Environment.NewLine, problematicOffers), ct);
    }
}
