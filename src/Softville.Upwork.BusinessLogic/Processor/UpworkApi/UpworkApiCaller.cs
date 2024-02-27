// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace Softville.Upwork.BusinessLogic.Processor.UpworkApi;

internal class UpworkApiCaller(IHttpClientFactory httpClientFactory, ILogger<UpworkApiCaller> logger) : IUpworkApiCaller
{
    public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken ct)
    {
        using HttpClient httpClient = httpClientFactory.CreateClient(UpworkHttpClient.UpworkClientName);

        var response = await httpClient.SendAsync(request, ct);

        if (response.IsSuccessStatusCode)
        {
            logger.LogDebug("{Uri} request succeeded", request.RequestUri);
        }
        else
        {
            logger.LogWarning("{Uri} request failed. {statusCode}: '{reason}' '{payload}'",
                request.RequestUri,
                response.StatusCode,
                response.ReasonPhrase,
                await response.Content.ReadAsStringAsync(ct));
        }

        return response;
    }
}
