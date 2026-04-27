namespace Atlas.SharedKernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Provides activation state management for entities that can be enabled or disabled
/// without being deleted.
/// </summary>
/// <remarks>
/// This interface is implemented by <c>MasterEntity&lt;TId&gt;</c>,
/// <c>ReferenceEntity&lt;TId&gt;</c>, and <c>TenantReferenceEntity&lt;TId&gt;</c>.
/// Inactive entities are excluded from lookup lists but are preserved for historical
/// referential integrity.
/// </remarks>
/// <example>
/// <code>
/// // Deactivating a currency that is no longer supported:
/// currency.Deactivate();
/// await unitOfWork.SaveChangesAsync();
///
/// // Filtering only active master records:
/// var activeCurrencies = await currencyRepo.ListAsync(c => c.IsActive);
/// </code>
/// </example>
public interface IActivatable
{
    /// <summary>
    /// Gets a value indicating whether this entity is currently active and
    /// available for selection in the system.
    /// </summary>
    bool IsActive { get; }

    /// <summary>Activates this entity, making it available for use throughout the system.</summary>
    void Activate();

    /// <summary>
    /// Deactivates this entity. It remains in the database for referential integrity
    /// but will be excluded from active lookups.
    /// </summary>
    void Deactivate();
}
