namespace ATLAS.Kernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Marks an entity as belonging to a specific tenant (organisation) in the
/// multi-tenant ATLAS architecture.
/// </summary>
/// <remarks>
/// <para>
/// Entities implementing this interface are subject to the global tenant query
/// filter registered by <c>AtlasDbContextBase</c>. Every query will automatically
/// include <c>WHERE TenantId = @currentTenantId</c> without any explicit filtering
/// in application code.
/// </para>
/// <para>
/// The tenant identifier is always a <see cref="Guid"/> regardless of the entity's
/// own primary key type, since tenants are managed globally across the entire system.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Verifying that a returned entity belongs to the current tenant:
/// ITenantAware entity = customer;
/// Debug.Assert(entity.TenantId == currentTenantContext.TenantId);
/// </code>
/// </example>
public interface ITenantAware
{
    /// <summary>
    /// Gets the unique identifier of the tenant (organisation) that owns this entity.
    /// </summary>
    Guid TenantId { get; }
}
