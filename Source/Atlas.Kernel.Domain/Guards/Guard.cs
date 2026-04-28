using ATLAS.Kernel.Domain.Guards.Internal;

namespace ATLAS.Kernel.Domain.Guards;

/// <summary>
/// Provides a fluent, composable API for defensive programming guard clauses.
/// Guard clauses validate preconditions and throw descriptive exceptions immediately,
/// preventing invalid state from propagating deep into the call stack.
/// </summary>
/// <remarks>
/// The entry point is the <see cref="Against"/> static property. All guard methods
/// return the validated value so they can be used inline in assignments.
/// </remarks>
/// <example>
/// <code>
/// // In a constructor or factory method:
/// public Customer(Guid id, string name, string email)
/// {
///     Id    = Guard.Against.Default(id,    nameof(id));
///     Name  = Guard.Against.NullOrWhiteSpace(name,  nameof(name));
///     Email = Guard.Against.NullOrWhiteSpace(email, nameof(email));
/// }
///
/// // Validating a numeric range:
/// Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
///
/// // Validating a collection:
/// Lines = Guard.Against.NullOrEmpty(lines, nameof(lines));
/// </code>
/// </example>
public static class Guard
{
    /// <summary>Gets the guard-clause builder. Use as the entry point for all validations.</summary>
    public static GuardClause Against { get; } = new GuardClause();
}
