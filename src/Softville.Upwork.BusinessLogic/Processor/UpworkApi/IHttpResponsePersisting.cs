using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor.UpworkApi;

internal interface IHttpResponsePersisting
{
    Task<HttpResponseMessage> PersistAsync(UpworkId id, IUpworkRequestType requestType, HttpResponseMessage response, CancellationToken ct);
}
