// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class Qualifications
{
    public int TotalQualifications { get; set; }
    public int MatchedQualifications { get; set; }
    public Qualification[] QualificationsDetails { get; set; } = Array.Empty<Qualification>();
}
