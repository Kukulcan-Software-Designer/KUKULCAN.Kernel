using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KUKULCAN.Kernel.Abstractions.Interfaces.Infrastructure;
using KUKULCAN.Kernel.Infrastructure.Behaviors;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Infrastructure.Behaviors;

public class CachingBehaviorTests
{
    [Fact]
    public void Type_IsSealed_Generic()
    {
        typeof(CachingBehavior<,>).IsSealed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_SkipsCache_WhenRequestIsNotCacheable()
    {
        var cache = new FakeCacheService();
        var behavior = new CachingBehavior<PlainRequest, string>(
            cache,
            NullLogger<CachingBehavior<PlainRequest, string>>.Instance);
        var nextCalls = 0;

        string result = await behavior.Handle(
            new PlainRequest(),
            _ =>
            {
                nextCalls++;
                return Task.FromResult("handler");
            },
            CancellationToken.None);

        result.Should().Be("handler");
        nextCalls.Should().Be(1);
        cache.SetCalls.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ReturnsCachedResponse_WhenCacheHit()
    {
        var cache = new FakeCacheService();
        cache.Values["key"] = "cached";
        var behavior = new CachingBehavior<CacheableRequest, string>(
            cache,
            NullLogger<CachingBehavior<CacheableRequest, string>>.Instance);
        var nextCalls = 0;

        string result = await behavior.Handle(
            new CacheableRequest("key", TimeSpan.FromMinutes(5)),
            _ =>
            {
                nextCalls++;
                return Task.FromResult("handler");
            },
            CancellationToken.None);

        result.Should().Be("cached");
        nextCalls.Should().Be(0);
        cache.SetCalls.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_CachesResponse_WhenCacheMiss()
    {
        var cache = new FakeCacheService();
        var behavior = new CachingBehavior<CacheableRequest, string>(
            cache,
            NullLogger<CachingBehavior<CacheableRequest, string>>.Instance);
        var duration = TimeSpan.FromMinutes(5);

        string result = await behavior.Handle(
            new CacheableRequest("key", duration),
            _ => Task.FromResult("handler"),
            CancellationToken.None);

        result.Should().Be("handler");
        cache.Values["key"].Should().Be("handler");
        cache.SetCalls.Should().ContainSingle(call => call.Key == "key" && call.Expiry == duration);
    }

    private sealed record PlainRequest : IRequest<string>;

    private sealed record CacheableRequest(string CacheKey, TimeSpan? CacheDuration)
        : IRequest<string>, ICacheableRequest;

    private sealed class FakeCacheService : ICacheService
    {
        public Dictionary<string, object?> Values { get; } = new();
        public List<(string Key, object? Value, TimeSpan? Expiry)> SetCalls { get; } = [];

        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Values.TryGetValue(key, out object? value) ? (T?)value : default);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
        {
            Values[key] = value;
            SetCalls.Add((key, value, expiry));
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            Values.Remove(key);
            return Task.CompletedTask;
        }

        public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan? expiry = null,
            CancellationToken cancellationToken = default)
        {
            if (Values.TryGetValue(key, out object? value))
                return (T)value!;

            T created = await factory(cancellationToken);
            await SetAsync(key, created, expiry, cancellationToken);
            return created;
        }

        public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default) =>
            Task.FromResult(Values.ContainsKey(key));

        public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            foreach (string key in Values.Keys.Where(k => k.StartsWith(prefix, StringComparison.Ordinal)).ToList())
                Values.Remove(key);
            return Task.CompletedTask;
        }
    }
}
