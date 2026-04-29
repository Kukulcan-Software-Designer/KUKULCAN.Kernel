namespace ATLAS.Kernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Marker interface for global master-data and reference entities that must
/// <b>never be deleted</b> (neither physically nor via soft-delete) from the system.
/// </summary>
/// <remarks>
/// <para>
/// Examples of entities that should implement this interface:
/// <list type="bullet">
///   <item><description>Countries (ISO 3166)</description></item>
///   <item><description>Currencies (ISO 4217)</description></item>
///   <item><description>Languages (BCP-47)</description></item>
///   <item><description>System-wide status codes (InvoiceStatus, OrderStatus)</description></item>
///   <item><description>Per-tenant configuration values (CustomerStatus, ProductCategory)</description></item>
/// </list>
/// </para>
/// <para>
/// The <c>IRepository&lt;T, TId&gt;</c> interface does not expose a <c>Delete</c>
/// method. Only <c>ISoftDeletableRepository&lt;T, TId&gt;</c> (which requires
/// <c>ISoftDeletable</c>) exposes <c>SoftDelete</c>. Since entities implementing
/// <see cref="IMasterData"/> never implement <c>ISoftDeletable</c>, delete operations
/// are structurally impossible at compile time.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // A Country entity that cannot be deleted:
/// public class Country : MasterEntity&lt;int&gt;
/// {
///     public string IsoCode { get; private set; } = string.Empty;
///     public string Name { get; private set; } = string.Empty;
/// }
/// // The repository for Country only exposes Add, Update (for IsActive), and queries.
/// // No Delete method is available.
/// </code>
/// </example>
public interface IMasterData { }
