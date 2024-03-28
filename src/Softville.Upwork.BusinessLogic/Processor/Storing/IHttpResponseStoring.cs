using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.Storing;

internal interface IHttpResponseStoring
{
    Task<HttpResponseMessage> PersistAsync(UpworkId id, IUpworkRequestType requestType, HttpResponseMessage response, CancellationToken ct);

    ValueTask PersistJsonAsync(UpworkId id, IUpworkRequestType requestType, string jsonString, CancellationToken ct);

    Task<string?> ReadAsync(UpworkId id, IUpworkRequestType requestType,
        CancellationToken ct);

    ValueTask<UpworkId[]> ListAllAsync(CancellationToken ct);
    Stream? Read(UpworkId id, IUpworkRequestType requestType);
}
