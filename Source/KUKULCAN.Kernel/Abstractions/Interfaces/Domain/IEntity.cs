namespace KUKULCAN.Kernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Defines the base contract for all persistent domain entities in the ATLAS system.
/// Every entity that is stored in the database must implement this interface to
/// guarantee a strongly-typed, non-null primary key.
/// </summary>
/// <typeparam name="TId">
/// The type of the entity's primary key.
/// Common choices are <see cref="Guid"/> (recommended), <see cref="int"/>,
/// or a custom strongly-typed identifier wrapper (e.g., <c>CustomerId</c>).
/// </typeparam>
/// <example>
/// <code>
/// // Defining a Customer entity with a Guid primary key:
/// public class Customer : TenantEntityBase&lt;Guid&gt;
/// {
///     public string Name { get; private set; } = string.Empty;
/// }
///
/// // Resolving an entity by its contract:
/// IEntity&lt;Guid&gt; entity = customer;
/// Console.WriteLine(entity.Id); // prints the customer's Guid
/// </code>
/// </example>
public interface IEntity<out TId> where TId : notnull
{
    /// <summary>
    /// Gets the unique identifier that distinguishes this entity from all others
    /// of the same type within the same storage scope.
    /// </summary>
    TId Id { get; }
}
