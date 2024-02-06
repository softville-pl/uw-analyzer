// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using Microsoft.Extensions.Logging;

namespace Softville.Upwork.BusinessLogic.Processor;

internal class UpworkProvider(ILogger<UpworkProvider> logger) : IUpworkProvider
{
    public async Task ProvideOffers(CancellationToken ct)
    {
        string outputFolder = @"d:\Misc\Temp\UpworkData\";

        using HttpClientHandler handler = new() { AutomaticDecompression = DecompressionMethods.All };

        using (HttpClient client = CreateClient(handler))
        {
            foreach (UpworkOfferItem offer in UpworkOfferItem.OffersToProcess)
            {
                string outputPath = Path.Combine(outputFolder, $"{offer.Id}.json");

                if (Path.Exists(outputPath))
                {
                    logger.LogWarning(@"'{offerId}{cipherText}' offer details exists. Skipping.", offer.Ciphertext, offer.Id);
                    continue;
                }

                HttpResponseMessage response =
                    await client.GetAsync(
                        $"https://www.upwork.com/job-details/jobdetails/api/job/{offer.Ciphertext}/summary", ct);

                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    await File.WriteAllTextAsync(outputPath, content);

                    logger.LogInformation("{offerId}{cipherText} request succeeded", offer.Id, offer.Ciphertext);
                }
                else
                {
                    logger.LogError("{offerId}{cipherText} request failed with status code:{statusCode}. Response: {content}",
                        offer.Id, offer.Ciphertext, response.StatusCode, content);
                }
            }
        }
    }

    private HttpClient CreateClient(HttpClientHandler handler)
    {
        HttpClient client = new(handler);
        client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.6");
        client.DefaultRequestHeaders.Add("Authorization", "Bearer oauth2v2_13195c38b98970a5b0ede90b582bbc56");
        client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        client.DefaultRequestHeaders.Add("Cookie",
            "visitor_id=185.132.92.59.1694087640295000; spt=e99624c5-0a32-4966-8d5a-68576bb3d474; recognized=j_romaniec; console_user=j_romaniec; DA_j_romaniec=70fd3c04e5a17fc869bbc949ae316b69286a3c81720b0acf046b741d0e4704bf; device_view=full; __cflb=02DiuEXPXZVk436fJfSVuuwDqLqkhavJabLpdsBHMzYmh; company_last_accessed=d14463776; current_organization_uid=829312457188573185; _cfuvid=X52I7DGgze6gnBuIqsnEkSg9MxEbYXKlMNcDLBRAjWw-1706951681700-0-604800000; user_uid=829312457184378880; master_access_token=9faf7e82.oauth2v2_2e5c8c2e3cb2fe8c73c4db1d8b569a58; oauth2_global_js_token=oauth2v2_13195c38b98970a5b0ede90b582bbc56; cookie_prefix=; cookie_domain=.upwork.com; country_code=AL; bcc19e16sb=oauth2v2_a09d81093a6655289751806500576a51; user_oauth2_slave_access_token=9faf7e82.oauth2v2_2e5c8c2e3cb2fe8c73c4db1d8b569a58:829312457184378880.oauth2v2_983e283cd94672c4edd1f8f23a4c35d9; channel=other; 8f7e80ebsb=oauth2v2_78173a4f571c3a55d47be3930da6d23b; _sp_id.2a16=dfcaf607-60ce-4c6b-ad0f-e8bfb9843570.1705731673.13.1706952007.1706870455.0cddbcea-4ac1-4e9d-b59b-ac9aab24fe9d; visitor_ui_gql_token_jobsearch=oauth2v2_458d3d6b491dcb2f47d7aff61441d451; UniversalSearchNuxt_vt=oauth2v2_2f3cad5afd66f35e0fde113ce2538c31; XSRF-TOKEN=18fcc5025b1082b8aa56075c03bc07ec; FindWorkHome:hasInterviewsOrOffers=0; FindWorkHome:freelancerMenuSpacing=230px; umq=1423; cf_chl_3=c9f4cb8fc8d15ab; cf_clearance=hFuFwFtnr9DRLBDWyRGKyUS0Swc2UPSXxBZIUsyI4JY-1706958047-1-AZzNrXw/abUBjCw3N8cIHkfkT/4oXx9CqxnUmRfLYL7Qa68oFAg3AyYSS5nInrsbyTiTYPfl+TGfjs3DVN3pLZ8=; vjd_gql_token=oauth2v2_beded76eea0203d025f0b97f65442ca7; __cf_bm=WnbIJjc6b0.2ppLcRzt5jOURHydU7Q0B38k37SpK7_0-1706958059-1-ARy4ZWuhUXt84phFivOPjf5p0y87DleC5VQbwrXo21aPMQ67ov7MV6cVW5OOCkoiw7p2Skl+vk/gqShnNEmZGnw=; _upw_ses.5831=*; forterToken=fa826f7ec7cb4af9bef0c81c87a0b8cc_1706958062037_113_UASb_14ck; enabled_ff=!RMTAir3Hired,!RMTAir3Home,CI9570Air2Dot5,SSINavUserBpa,CI11132Air2Dot75,!CI10857Air3Dot0,!CI12577UniversalSearch,!i18nGA,!CLOBJPGV2RJP,!MP16400Air3Migration,i18nOn,CLOBSNAIR3,TONB2256Air3Migration,OTBnrOn,JPAir3,!air2Dot76Qt,CLOBJNAIR3,FLSAir3,!RMTAir3Talent,!CI10270Air2Dot5QTAllocations,!SSINavUser,air2Dot76; _upw_id.5831=54b53d1d-56b2-4fe0-89cd-98a7d796ff9a.1702102471.75.1706958341.1706955544.6f97028e-037b-4a20-8da3-7c79b3c527c6.a0198f89-3e6c-4340-b3d1-e0e4a7a5d92a.8f3eca15-f6e2-4333-b79d-4ad5cd5f8eab.1706958062133.15; AWSALB=sZU0bAnKf14y3wY20QIDU0o4KL1g4Promb0umkPHZ+6s8S2Wk/BbnmTTe2nz1QmIINlrLFemIkR6Nymbiqir2Kphsb9cmzSD/u66/hPrfWcoJQ4MsGlfLdSrYKG+; AWSALBCORS=sZU0bAnKf14y3wY20QIDU0o4KL1g4Promb0umkPHZ+6s8S2Wk/BbnmTTe2nz1QmIINlrLFemIkR6Nymbiqir2Kphsb9cmzSD/u66/hPrfWcoJQ4MsGlfLdSrYKG+");
        client.DefaultRequestHeaders.Add("Pragma", "no-cache");
        client.DefaultRequestHeaders.Add("Referer",
            "https://www.upwork.com/nx/search/jobs/details/~0133110211e88e3a06?contractor_tier=3&hourly_rate=50-&per_page=50&q=.net&sort=recency&t=0&saved_jobs_page=4&pageTitle=Job%20Details&_navType=slider&_modalInfo=%5B%7B%22navType%22%3A%22slider%22,%22title%22%3A%22Job%20Details%22,%22modalId%22%3A%221706952059223%22%7D%5D");
        client.DefaultRequestHeaders.Add("Sec-Ch-Ua",
            "\"Google Chrome\";v=\"118\", \"Chromium\";v=\"118\", \"Not=A?Brand\";v=\"24\"");
        client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
        client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
        client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
        client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
        client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
        client.DefaultRequestHeaders.Add("Sec-Gpc", "1");
        client.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("Vnd-Eo-Parent-Span-Id", "e37defb8-32ae-478a-b353-1d375fd0094e");
        client.DefaultRequestHeaders.Add("Vnd-Eo-Span-Id", "8e8ada4b-ffa5-49f0-a0a6-3c8ccc314809");
        client.DefaultRequestHeaders.Add("Vnd-Eo-Trace-Id", "84f994c06d8b71a3-BEG");
        client.DefaultRequestHeaders.Add("X-Odesk-User-Agent", "oDesk LM");
        client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        client.DefaultRequestHeaders.Add("X-Upwork-Accept-Language", "en-US");

        return client;
    }
}
