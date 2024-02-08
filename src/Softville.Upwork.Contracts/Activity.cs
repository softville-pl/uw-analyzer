// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class Activity
{
    public DateTime LastBuyerActivity { get; set; }
    public int TotalApplicants { get; set; }
    public int TotalHired { get; set; }
    public int TotalInvitedToInterview { get; set; }
    public int UnansweredInvites { get; set; }
    public int InvitationsSent { get; set; }
}
