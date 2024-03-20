// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Softville.Upwork.Tests.Common;

namespace Softville.Upwork.WebApi.IntTests.Infrastructure;

public class WebApiTestBase(WebApiContext ctx) : IntTestBase<WebApiContext>(ctx)
{ }
