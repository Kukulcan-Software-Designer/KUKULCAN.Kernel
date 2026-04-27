namespace Atlas.SharedKernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Marker interface for append-only entities that must never be modified or
/// deleted once they have been inserted into the database.
/// </summary>
/// <remarks>
/// <para>
/// Apply this marker to entities whose full history must be preserved with
/// absolute integrity, such as:
/// <list type="bullet">
///   <item><description>Persisted domain event records</description></item>
///   <item><description>Audit log entries</description></item>
///   <item><description>Financial journal lines (double-entry ledger)</description></item>
///   <item><description>Digital signature records</description></item>
/// </list>
/// </para>
/// <para>
/// The infrastructure layer uses this marker to prevent EF Core from emitting
/// <c>UPDATE</c> or <c>DELETE</c> statements for these entities.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuditLogEntry : EntityBase&lt;Guid&gt;, IImmutable
/// {
///     public string Action { get; private set; } = string.Empty;
///     public DateTimeOffset OccurredAt { get; private set; }
/// }
/// </code>
/// </example>
public interface IImmutable { }
