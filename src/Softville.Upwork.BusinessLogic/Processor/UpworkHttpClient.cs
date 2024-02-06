// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Softville.Upwork.BusinessLogic.Processor.Configuration;

namespace Softville.Upwork.BusinessLogic.Processor;

internal static class UpworkHttpClient
{
    internal const string Name = "Upwork";

    internal static void Configure(IServiceProvider serviceProvider, HttpClient client)
    {
        IOptions<UpworkConfig> upworkConfig = serviceProvider.GetRequiredService<IOptions<UpworkConfig>>();

        client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.6");
        client.DefaultRequestHeaders.Add("Authorization", "Bearer oauth2v2_13195c38b98970a5b0ede90b582bbc56");
        client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        client.DefaultRequestHeaders.Add("Cookie", upworkConfig.Value.Cookie);
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
    }
}
