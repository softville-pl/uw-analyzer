namespace Softville.Upwork.Contracts;

public class Offer
{
    public required string Title { get; set; }
    public required string Uid { get; set; }
    public required long Rid { get; set; }
    public required Cooperation Cooperation { get; set; }
    public required string CipherText { get; set; }
    public required string Category { get; set; }
    public required string CategoryGroup { get; set; }
    public DateTime PublishedTime { get; set; }
    public required Rate Rate { get; set; }
    public required string LinkUrl { get; set; }
    public required Developer Developers { get; set; }
    public required Customer Customer { get; set; }
    public required string BusinessDomain { get; set; }
    public required Technology Technology { get; set; }
    public required string ApplicationProcess { get; set; }
    public required string UnusualRequirements { get; set; }
    public required string[] Requirements { get; set; }
    public required string Duties { get; set; }
    public required string[] Questions { get; set; }
    public int ConnectPrice { get; set; }
}
