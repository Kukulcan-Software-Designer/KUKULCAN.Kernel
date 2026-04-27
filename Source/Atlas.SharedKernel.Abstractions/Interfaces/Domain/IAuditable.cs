namespace Atlas.SharedKernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Provides auditing metadata for entities that must track creation and modification
/// history. All entities inheriting from <c>AuditableEntityBase&lt;TId&gt;</c>
/// implement this interface automatically.
/// </summary>
/// <remarks>
/// The <c>AuditSaveChangesInterceptor</c> in <c>Atlas.Database</c> populates
/// these fields automatically on every save operation — no manual assignment
/// is required in application code.
/// </remarks>
/// <example>
/// <code>
/// // Reading audit information:
/// Console.WriteLine($"Created by {entity.CreatedBy} on {entity.CreatedAt:yyyy-MM-dd}");
/// if (entity.UpdatedAt.HasValue)
///     Console.WriteLine($"Last updated by {entity.UpdatedBy} on {entity.UpdatedAt:yyyy-MM-dd}");
/// </code>
/// </example>
public interface IAuditable
{
    /// <summary>Gets the UTC timestamp at which this entity was first persisted.</summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>Gets the identifier of the user or system that created this entity.</summary>
    string CreatedBy { get; }

    /// <summary>
    /// Gets the UTC timestamp of the most recent update to this entity,
    /// or <c>null</c> if the entity has never been updated since creation.
    /// </summary>
    DateTimeOffset? UpdatedAt { get; }

    /// <summary>
    /// Gets the identifier of the user or system that last updated this entity,
    /// or <c>null</c> if the entity has never been updated.
    /// </summary>
    string? UpdatedBy { get; }
}
