// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

[CollectionDefinition(Name, DisableParallelization = true)]
public class IntPrpCollection : ICollectionFixture<IntPrpContext>
{
    public const string Name = nameof(IntPrpContext);
}
