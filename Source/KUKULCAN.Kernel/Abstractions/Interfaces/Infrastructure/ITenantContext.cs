namespace KUKULCAN.Kernel.Abstractions.Interfaces.Infrastructure;

/// <summary>
/// Provides the resolved tenant context for the current request.
/// The tenant is resolved by <c>TenantMiddleware</c> from the hostname,
/// <c>X-Tenant-Id</c> header, JWT claim, or query parameter before the
/// request reaches the MediatR pipeline.
/// </summary>
/// <example>
/// <code>
/// // Reading tenant locale for formatting:
/// string formatted = tenantContext.Locale switch
/// {
///     "es-ES" => amount.ToString("C", new CultureInfo("es-ES")),
///     "en-US" => amount.ToString("C", new CultureInfo("en-US")),
///     _       => amount.ToString("C")
/// };
/// </code>
/// </example>
public interface ITenantContext
{
    /// <summary>Gets the globally unique identifier of the resolved tenant.</summary>
    Guid TenantId { get; }

    /// <summary>Gets the human-readable code of the tenant (e.g., <c>"ACME"</c>).</summary>
    string TenantCode { get; }

    /// <summary>
    /// Gets the BCP-47 locale tag for the tenant's default locale
    /// (e.g., <c>"es-ES"</c>, <c>"en-US"</c>, <c>"ca-ES"</c>).
    /// </summary>
    string Locale { get; }

    /// <summary>
    /// Gets the IANA time zone identifier for the tenant
    /// (e.g., <c>"Europe/Madrid"</c>, <c>"America/New_York"</c>).
    /// </summary>
    string TimeZoneId { get; }

    /// <summary>
    /// Gets the ISO 4217 currency code for the tenant's default currency
    /// (e.g., <c>"EUR"</c>, <c>"USD"</c>, <c>"GBP"</c>).
    /// </summary>
    string DefaultCurrencyCode { get; }

    /// <summary>
    /// Gets a value indicating whether the tenant context has been successfully
    /// resolved from the current request. Returns <c>false</c> in background jobs
    /// or unauthenticated requests.
    /// </summary>
    bool IsResolved { get; }
}
