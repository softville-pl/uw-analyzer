﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Softville.Upwork.BusinessLogic.Processor.Storing;
using Softville.Upwork.Tests.Common.Stubs;

namespace Softville.Upwork.Tests.Common.Components;

public class WebJobServicesComponent : IDisposable
{
    private IServiceScope? _scopedProvider;
    private IServiceProvider RootProvider { get; }

    private IServiceProvider ScopedProvider =>
        _scopedProvider?.ServiceProvider ?? throw new InvalidOperationException("Scoped provider hasn't been initialized");

    public WebJobServicesComponent(IServiceProvider rootProvider)
    {
        RootProvider = rootProvider;
    }

    public void InitScope()
    {
        _scopedProvider?.Dispose();
        _scopedProvider = RootProvider.CreateAsyncScope();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _scopedProvider?.Dispose();
            _scopedProvider = null;
        }
    }

    public InMemoryStoringStub ResponseStoring =>
        (InMemoryStoringStub)ScopedProvider.GetRequiredService<IHttpResponseStoring>();

    public T GetRequiredService<T>() where T : notnull
    {
        return ScopedProvider.GetRequiredService<T>();
    }
}
