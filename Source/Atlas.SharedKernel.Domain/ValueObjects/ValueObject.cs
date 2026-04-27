namespace Atlas.SharedKernel.Domain.ValueObjects;

/// <summary>
/// Abstract base class for value objects in the DDD sense.
/// Value objects have no identity — they are equal when all their component
/// values are equal. They are also immutable by design.
/// </summary>
/// <remarks>
/// <para>
/// Override <see cref="GetEqualityComponents"/> to return the sequence of
/// properties that define equality for the concrete value object.
/// Do not include mutable state in the equality components.
/// </para>
/// <para>
/// Use EF Core's <c>OwnsOne</c> / <c>OwnsMany</c> to map value objects to
/// columns within the owning entity's table (no separate table required).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public sealed class Address : ValueObject
/// {
///     public string Street { get; }
///     public string City { get; }
///     public string PostalCode { get; }
///
///     private Address(string street, string city, string postalCode)
///     {
///         Street = street; City = city; PostalCode = postalCode;
///     }
///
///     public static Result&lt;Address&gt; Create(string street, string city, string postalCode)
///     {
///         if (string.IsNullOrWhiteSpace(street))
///             return Error.Validation("Address.Street.Empty", "Street must not be empty.");
///         return new Address(street, city, postalCode);
///     }
///
///     protected override IEnumerable&lt;object?&gt; GetEqualityComponents()
///     {
///         yield return Street.ToUpperInvariant();
///         yield return City.ToUpperInvariant();
///         yield return PostalCode.ToUpperInvariant();
///     }
/// }
/// </code>
/// </example>
public abstract class ValueObject
{
    /// <summary>
    /// Returns the sequence of values used to determine equality.
    /// Two value objects with identical components are considered equal.
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        return GetEqualityComponents()
            .SequenceEqual(((ValueObject)obj).GetEqualityComponents());
    }

    /// <inheritdoc/>
    public override int GetHashCode() =>
        GetEqualityComponents()
            .Aggregate(
                new HashCode(),
                (hc, component) => { hc.Add(component); return hc; })
            .ToHashCode();

    /// <summary>Determines whether two value objects are equal.</summary>
    public static bool operator ==(ValueObject? left, ValueObject? right) =>
        left is null ? right is null : left.Equals(right);

    /// <summary>Determines whether two value objects are not equal.</summary>
    public static bool operator !=(ValueObject? left, ValueObject? right) =>
        !(left == right);
}
