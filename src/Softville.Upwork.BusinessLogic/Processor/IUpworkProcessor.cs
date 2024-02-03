// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Softville.Upwork.BusinessLogic.Processor;

public interface IUpworkProcessor
{
    [RequiresDynamicCode(
        "Calls Softville.Upwork.BusinessLogic.Processor.UpworkParser.ParseAsync(Stream, CancellationToken)")]
    [RequiresUnreferencedCode(
        "Calls Softville.Upwork.BusinessLogic.Processor.UpworkParser.ParseAsync(Stream, CancellationToken)")]
    Task ProcessOffersAsync(CancellationToken ct);
}
