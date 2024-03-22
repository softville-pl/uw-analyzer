// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.Processor.Migrator;

public interface IOffersMigrator
{
    Task MigrateAsync(CancellationToken ct);
}
