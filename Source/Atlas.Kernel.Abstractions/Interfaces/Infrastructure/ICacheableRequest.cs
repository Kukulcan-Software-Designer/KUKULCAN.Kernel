namespace ATLAS.Kernel.Abstractions.Interfaces.Infrastructure;

/// <summary>
/// Marks a MediatR query as eligible for response caching by the
/// <c>CachingBehavior</c> pipeline behavior.
/// </summary>
/// <remarks>
/// Only <b>queries</b> (read operations) should implement this interface.
/// Commands that modify state must never be cached.
/// </remarks>
/// <example>
/// <code>
/// public sealed record GetCustomerListQuery(Guid TenantId, int Page, int PageSize)
///     : IRequest&lt;Result&lt;PagedResult&lt;CustomerDto&gt;&gt;&gt;, ICacheableRequest
/// {
///     public string CacheKey => $"crm:customers:{TenantId}:p{Page}:s{PageSize}";
///     public TimeSpan? CacheDuration => TimeSpan.FromMinutes(5);
/// }
/// </code>
/// </example>
public interface ICacheableRequest
{
    /// <summary>Gets the cache key uniquely identifying this request's cached response.</summary>
    string CacheKey { get; }

    /// <summary>
    /// Gets the cache entry's time-to-live, or <c>null</c> to use the system default.
    /// </summary>
    TimeSpan? CacheDuration { get; }
}
