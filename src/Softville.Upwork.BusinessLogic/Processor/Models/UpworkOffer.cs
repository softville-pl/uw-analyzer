namespace Softville.Upwork.BusinessLogic;

public class UpworkOffer
{
    public required Job Job { get; set; }
    public Buyer? Buyer { get; set; }
    public CurrentUserInfo? CurrentUserInfo { get; set; }
    public Paths? Paths { get; set; }
    public FeatureFlags? Ff { get; set; }
    public Experiments? Experiments { get; set; }
    public bool IsVisitor { get; set; }
}

public class Job
{
    public required string Ciphertext { get; set; }
    public long Rid { get; set; }
    public string? Uid { get; set; }
    public int Type { get; set; }
    public int Access { get; set; }
    public string? Title { get; set; }
    public int Status { get; set; }
    public required Category Category { get; set; }
    public required CategoryGroup CategoryGroup { get; set; }
    public bool HideBudget { get; set; }
    public Budget? Budget { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime PostedOn { get; set; }
    public DateTime PublishTime { get; set; }
    public bool WasRenewed { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Workload { get; set; }
    public int DurationIdV3 { get; set; }
    public string? DurationLabel { get; set; }
    public bool NotSureProjectDuration { get; set; }
    public bool NotSureFreelancersToHire { get; set; }
    public bool NotSureExperienceLevel { get; set; }
    public int NumberOfPositionsToHire { get; set; }
    public ExtendedBudgetInfo? ExtendedBudgetInfo { get; set; }
    public int ContractorTier { get; set; }
    public string? Description { get; set; }
    public int AttachmentsCount { get; set; }
    public List<QuestionDetail>? Questions { get; set; }
    public Qualifications? Qualifications { get; set; }
    public bool IsPremium { get; set; }
    public List<SegmentationData>? SegmentationData { get; set; }
    public required ClientActivity ClientActivity { get; set; }
    public Annotations? Annotations { get; set; }
    public bool IsContractToHire { get; set; }
}

public class Category
{
    public required string Name { get; set; }
    public string? UrlSlug { get; set; }
}

public class CategoryGroup
{
    public required string Name { get; set; }
    public string? UrlSlug { get; set; }
}

public class Budget
{
    public string? CurrencyCode { get; set; }
    public decimal Amount { get; set; }
}

public class ExtendedBudgetInfo
{
    public int HourlyBudgetType { get; set; }
    public decimal HourlyBudgetMin { get; set; }
    public decimal HourlyBudgetMax { get; set; }
}

public class QuestionDetail
{
    public int Position { get; set; }
    public string? Question { get; set; }
}

public class Qualifications
{
    public bool ShouldHavePortfolio { get; set; } //
    public int MinHoursWeek { get; set; }
    public int MinJobSuccessScore { get; set; }
    public bool RisingTalent { get; set; }
    public bool LocationCheckRequired { get; set; }
    public bool LocalMarket { get; set; }
}

public class SegmentationData
{
    public string? Name { get; set; }
    public string? Value { get; set; }
    public string? Label { get; set; }
    public string? Type { get; set; }
    public int SortOrder { get; set; }
    public string? TypeUid { get; set; }
    public string? CustomValue { get; set; }
    public string? Skill { get; set; }
}

public class ClientActivity
{
    public DateTime? LastBuyerActivity { get; set; }
    public int? TotalApplicants { get; set; }
    public int? TotalHired { get; set; }
    public int? TotalInvitedToInterview { get; set; }
    public int? UnansweredInvites { get; set; }
    public int? InvitationsSent { get; set; }
}

public class Annotations
{
    public List<string>? Tags { get; set; }
    public CustomFields? CustomFields { get; set; }
}

public class CustomFields
{
    public string? SiteSource { get; set; }
    public string? PublishTime { get; set; }
    public string? TotalTimeJobPostFlowAIv2 { get; set; }
    public string? SourcingUpdateCount { get; set; }
    public string? SourcingUpdateForbidden { get; set; }
    public string? TotalTimeSpentOnReviewPageAIv2 { get; set; }
    public string? Type { get; set; }
    public string? FinalInputsAIv2 { get; set; }
    public string? JpgV2Prompt { get; set; }
    public string? Browser { get; set; }
    public string? TimeSpentToGetAPIDescriptionAI { get; set; }
    public string? Device { get; set; }
    public string? StartTimeJobPostFlowAIv2 { get; set; }
    public string? GeneratedDescriptionAI { get; set; }
    public string? InputsSkillsAI { get; set; }
    public string? TotalDescriptionEditingTimeAI { get; set; }
    public string? InputsDurationAI { get; set; }
    public string? InputsTitleAI { get; set; }
    public string? FinalDescriptionAI { get; set; }
}

public class Buyer
{
    public bool IsPaymentMethodVerified { get; set; }
    public Location? Location { get; set; }
    public Stats? Stats { get; set; }
    public bool IsEnterprise { get; set; }
    public Company? Company { get; set; }
    public Jobs? Jobs { get; set; }
    public AvgHourlyJobsRate? AvgHourlyJobsRate { get; set; }
}

public class Location
{
    public string? City { get; set; }
    public string? Country { get; set; }
}

public class Stats
{
    public int TotalAssignments { get; set; }
    public int ActiveAssignmentsCount { get; set; }
    public int FeedbackCount { get; set; }
    public double Score { get; set; }
    public int TotalJobsWithHires { get; set; }
    public double HoursCount { get; set; }
    public TotalCharges? TotalCharges { get; set; }
}

public class TotalCharges
{
    public string? CurrencyCode { get; set; }
    public decimal Amount { get; set; }
}

public class Company
{
    public bool IsEDCReplicated { get; set; }
    public DateTime? ContractDate { get; set; }
    public Profile? Profile { get; set; }
}

public class Profile
{
    public string? Size { get; set; }
    public string? Industry { get; set; }
    public bool Visible { get; set; }
}

public class Jobs
{
    public int PostedCount { get; set; }
    public int OpenCount { get; set; }
}

public class AvgHourlyJobsRate
{
    public decimal Amount { get; set; }
}

public class CurrentUserInfo
{
    public bool Owner { get; set; }
    public FreelancerInfo? FreelancerInfo { get; set; }
}

public class QualificationsMatches
{
    public int TotalQualifications { get; set; }
    public int TotalMatches { get; set; }
    public List<Match>? Matches { get; set; }
}

public class Match
{
    public int Qualification { get; set; }
    public bool Qualified { get; set; }
    public string? ClientPreferred { get; set; }
    public string? FreelancerValue { get; set; }
    public string? FreelancerValueLabel { get; set; }
    public string? ClientPreferredLabel { get; set; }
}

public class FreelancerInfo
{
    public QualificationsMatches? QualificationsMatches { get; set; }
    public int ProfileState { get; set; }
    public string? DevProfileCiphertext { get; set; }
}

public class Paths
{
    public string? Js { get; set; }
}

public class FeatureFlags
{
    public bool IsRemoveClientTotalSpentCap { get; set; }
    public bool IsJobDetailsQuickJobPost { get; set; }
    public bool CFE6770ImprovedQualificationsSection { get; set; }
    public bool JdShowInvitesCount { get; set; }
    public bool EnforceMinimumRateInProfile { get; set; }
    public bool TSV1877IDVatSubmitProposal { get; set; }
    public bool AG2156TeamsAddRoleFlow { get; set; }
    public bool CLOB6375ExpandedCompanyProfile { get; set; }
}

public class Experiments
{
    public IdvOnSubmitProposals? IdvOnSubmitProposals { get; set; }
}

public class IdvOnSubmitProposals
{
    public bool Soft { get; set; }
    public bool Hard { get; set; }
    public bool Idv_Required { get; set; }
    public bool Idv_Complete { get; set; }
}
