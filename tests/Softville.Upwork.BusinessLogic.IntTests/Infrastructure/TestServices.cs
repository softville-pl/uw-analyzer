// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;

namespace Softville.Upwork.BusinessLogic.IntTests.Infrastructure;

public class TestServices : IDisposable
{
    private IServiceScope? _scopedProvider;
    private IServiceProvider RootProvider { get; }

    private IServiceScope ScopedProvider =>
        _scopedProvider ?? throw new InvalidOperationException("Scoped provider hasn't been initialized");

    public TestServices(IServiceProvider rootProvider)
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

    public T GetRequiredService<T>() where T : notnull
    {
        return ScopedProvider.ServiceProvider.GetRequiredService<T>();
    }
}
