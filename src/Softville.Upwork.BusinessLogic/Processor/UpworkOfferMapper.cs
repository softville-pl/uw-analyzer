// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Contracts;

namespace Softville.Upwork.BusinessLogic.Processor;

internal static class UpworkOfferMapper
{
    internal const string NA = "N/A";

    internal static Offer MapToOffer(this UpworkOffer upworkOffer)
    {
        Job job = upworkOffer.Job;

        return new Offer
        {
            Title = job.Title ?? NA,
            Uid = job.Uid ?? NA,
            Rid = job.Rid,
            Cooperation = job.MapToCooperation(),
            CipherText = job.Ciphertext,
            Category = job.Category.Name,
            PublishedTime = job.PublishTime,
            CategoryGroup = job.CategoryGroup.Name,
            Rate = job.ExtendedBudgetInfo.MapToRate(),
            LinkUrl = job.Ciphertext,
            Developers = new Developer(),
            Customer = upworkOffer.Buyer.MapToCustomer(),
            BusinessDomain = NA,
            Technology =
                new Technology
                {
                    Architecture = NA,
                    DotnetVersion = NA,
                    BackendFrameworks = NA,
                    FrontendFrameworks = NA,
                    Databases = NA,
                    Cloud = new Cloud {Provider = NA, Services = NA},
                    ThirdParty = NA
                },
            ApplicationProcess = NA,
            UnusualRequirements = NA,
            Requirements = Array.Empty<string>(),
            ConnectPrice = 0,
            Duties = NA,
            Questions = job.Questions?.Select(q => q.Question ?? NA)?.ToArray() ?? Array.Empty<string>(),
            Activity = job.ClientActivity.MapToActivity()
        };
    }

    internal static Activity MapToActivity(this ClientActivity activity) =>
        new()
        {
            LastBuyerActivity = activity.LastBuyerActivity,
            TotalApplicants = activity.TotalApplicants,
            TotalHired = activity.TotalHired,
            TotalInvitedToInterview = activity.TotalInvitedToInterview,
            UnansweredInvites = activity.UnansweredInvites,
            InvitationsSent = activity.InvitationsSent
        };

    internal static Rate MapToRate(this ExtendedBudgetInfo? budget) =>
        new() {Currency = "USD", Maximum = budget?.HourlyBudgetMax ?? -1, Minimum = budget?.HourlyBudgetMin ?? -1};

    internal static Customer MapToCustomer(this Buyer? buyer)
    {
        Stats? stats = buyer?.Stats;
        return new Customer
        {
            Location =
                new Contracts.Location {Country = buyer?.Location?.Country ?? NA, City = buyer?.Location?.City ?? NA},
            Profile =
                new Contracts.Profile
                {
                    Industry = buyer?.Company?.Profile?.Industry ?? NA, Size = buyer?.Company?.Profile?.Size ?? NA
                },
            Stats = new Contracts.Stats
            {
                ActiveAssignmentsCount = stats?.ActiveAssignmentsCount,
                Score = stats?.Score ?? 0,
                FeedbackCount = stats?.FeedbackCount ?? 0,
                HoursCount = stats?.HoursCount ?? 0,
                TotalAssignments = stats?.TotalAssignments ?? 0,
                TotalJobsWithHires = stats?.TotalJobsWithHires ?? 0,
                TotalCharges = new Contracts.TotalCharges
                {
                    Amount = stats?.TotalCharges?.Amount ?? 0,
                    CurrencyCode = stats?.TotalCharges?.CurrencyCode ?? NA
                }
            }
        };
    }

    internal static Cooperation MapToCooperation(this Job job) =>
        new()
        {
            Workload = job.Workload ?? NA,
            DurationLabel = job.DurationLabel ?? NA,
            DeliveryDate = job.DeliveryDate,
            NumberOfPositionsToHire = job.NumberOfPositionsToHire,
            StartDate = job.StartDate
        };
}
