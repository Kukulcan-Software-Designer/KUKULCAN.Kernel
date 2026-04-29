namespace ATLAS.Kernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Marks an entity as supporting soft-deletion.
/// Entities implementing this interface are <b>never physically removed</b> from the
/// database. Instead, <see cref="IsDeleted"/> is set to <c>true</c> and the
/// <see cref="DeletedAt"/> / <see cref="DeletedBy"/> audit fields are populated.
/// </summary>
/// <remarks>
/// <para>
/// The <c>AtlasDbContextBase</c> automatically registers a global EF Core
/// <c>HasQueryFilter</c> for every entity implementing <see cref="ISoftDeletable"/>,
/// so all normal queries transparently exclude deleted records.
/// </para>
/// <para>
/// Only entities inheriting from <c>TenantEntityBase&lt;TId&gt;</c> implement this
/// interface. <c>MasterEntity</c>, <c>ReferenceEntity</c>, and
/// <c>TenantReferenceEntity</c> deliberately do <b>not</b> implement it.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Checking soft-delete state:
/// if (customer.IsDeleted)
///     Console.WriteLine($"Deleted by {customer.DeletedBy} at {customer.DeletedAt}");
/// </code>
/// </example>
public interface ISoftDeletable
{
    /// <summary>Gets a value indicating whether this entity has been soft-deleted.</summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets the UTC timestamp at which this entity was soft-deleted, or <c>null</c>
    /// if the entity has not been deleted.
    /// </summary>
    DateTimeOffset? DeletedAt { get; }

    /// <summary>
    /// Gets the identifier (user name or system identifier) of the principal that
    /// performed the soft-deletion, or <c>null</c> if not deleted.
    /// </summary>
    string? DeletedBy { get; }

    /// <summary>
    /// Marks this entity as soft-deleted, recording who deleted it and when.
    /// </summary>
    /// <param name="deletedBy">
    /// The identifier of the user or system process performing the deletion.
    /// Must not be null or whitespace.
    /// </param>
    /// <param name="deletedAt">The UTC timestamp of the deletion.</param>
    void MarkAsDeleted(string deletedBy, DateTimeOffset deletedAt);

    /// <summary>
    /// Restores a previously soft-deleted entity, clearing all deletion metadata.
    /// </summary>
    void Restore();
}
