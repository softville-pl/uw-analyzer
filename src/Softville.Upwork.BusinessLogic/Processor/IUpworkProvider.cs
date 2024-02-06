namespace Softville.Upwork.BusinessLogic.Processor;

public interface IUpworkProvider {
    Task ProvideOffers(CancellationToken ct);
}