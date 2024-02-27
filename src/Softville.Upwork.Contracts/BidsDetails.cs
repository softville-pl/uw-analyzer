// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class BidsDetails
{
    public static readonly BidsDetails Empty = new() {Amount = -1, CurrencyCode = "??"};

    public required string CurrencyCode { get; set; }
    public decimal Amount { get; set; }
}
