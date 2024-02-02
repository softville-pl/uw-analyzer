// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class Customer
{
    public required Location Location { get; set; }
    public required Profile Profile { get; set; }
}
