namespace Softville.Upwork.Contracts;

public class Offer
{
    public required string Title { get; set; }
    public required int Uid { get; set; }
    public required int Rid { get; set; }
    public required Cooperation Cooperation { get; set; }
    public required string CipherText { get; set; }
    public required string Category { get; set; }
    public required string CategoryGroup { get; set; }
    public DateTime Date { get; set; }
    public required Rate Rate { get; set; }
    public required string LinkUrl { get; set; }
    public required Developer Developers { get; set; }
    public required Customer Customer { get; set; }
    public required string BusinessDomain { get; set; }
    public required Technology Technology { get; set; }
    public required string ApplicationProcess { get; set; }
    public required string UnusualRequirements { get; set; }
    public required string Duties { get; set; }
    public required string[] Questions { get; set; }
}

public class Cooperation
{
    public required string DurationLabel { get; set; }
    public required string Workload { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? NumberOfPositionsToHire { get; set; }
}
