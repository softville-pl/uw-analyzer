// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace Softville.Upwork.BusinessLogic;

public class UpworkSearchResult
{
    public required SearchResults SearchResults { get; set; }
}

public class SearchResults
{
    public required string Q { get; set; }
    public required Paging Paging { get; set; }
    public required List<JobSearch> Jobs { get; set; }
}

public class Paging
{
    public int Total { get; set; }
    public int Offset { get; set; }
    public int Count { get; set; }
}

public class JobSearch
{
    public required string Title { get; set; }
    public required string Ciphertext { get; set; }
    public required string Description { get; set; }

    //ToDo consider using original Uid name
    [JsonPropertyName("Uid")] public required string Id { get; set; }

    public int? FreelancersToHire { get; set; }
    public DateTime? PublishedOn { get; set; }
    public required List<Attr> Attrs { get; set; }
    public bool? IsLocal { get; set; }
    public int? ConnectPrice { get; set; }
}

public class Attr
{
    public required string PrettyName { get; set; }
}
