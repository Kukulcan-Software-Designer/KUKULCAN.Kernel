namespace KUKULCAN.Kernel.Abstractions.Interfaces.Infrastructure;

/// <summary>
/// Provides a distributed cache abstraction backed by Redis in production
/// and an in-memory implementation in development/testing environments.
/// </summary>
/// <remarks>
/// Cache keys should include the tenant identifier where relevant to prevent
/// cross-tenant data leakage (e.g., <c>$"crm:customer:{tenantId}:{customerId}"</c>).
/// </remarks>
/// <example>
/// <code>
/// // Cache-aside pattern with automatic expiry:
/// var customer = await cacheService.GetOrCreateAsync(
///     key: $"crm:customer:{tenantId}:{customerId}",
///     factory: ct => customerRepo.GetByIdAsync(customerId, ct)!,
///     expiry: TimeSpan.FromMinutes(10),
///     cancellationToken: ct);
/// </code>
/// </example>
public interface ICacheService
{
    /// <summary>
    /// Retrieves a cached value by key, or <c>null</c> if the key does not exist
    /// or has expired.
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>Stores a value in the cache with an optional expiry duration.</summary>
    /// <param name="key">The cache key. Must be unique within the application scope.</param>
    /// <param name="value">The value to cache. Must be serializable.</param>
    /// <param name="expiry">
    /// The time-to-live for this cache entry. When <c>null</c>, the entry
    /// persists until explicitly removed or the cache is flushed.
    /// </param>
    /// <param name="cancellationToken">Propagates notification that the operation should be canceled.</param>
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default);

    /// <summary>Removes a cached entry by key. No-op if the key does not exist.</summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the cached value if present; otherwise executes the factory,
    /// stores the result, and returns it.
    /// </summary>
    Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default);

    /// <summary>Checks whether a cache entry exists for the given key.</summary>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Removes all cache entries matching the given key prefix.</summary>
    Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
}
