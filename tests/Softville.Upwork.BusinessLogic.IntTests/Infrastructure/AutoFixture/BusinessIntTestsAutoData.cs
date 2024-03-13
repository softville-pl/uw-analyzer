// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoFixture.Xunit2;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure.AutoFixture;

public class BusinessIntTestsAutoData() : AutoDataAttribute(() => new BusinessIntTestsFixture());
