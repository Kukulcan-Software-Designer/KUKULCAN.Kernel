namespace Atlas.SharedKernel.Domain.Specifications;

/// <summary>
/// Abstract base class for the Specification pattern. Encapsulates a query
/// predicate as an Expression&lt;Func&lt;T, bool&gt;&gt; that can be passed
/// directly to EF Core's <c>Where()</c> for translated SQL, or evaluated
/// in-memory via IsSatisfiedBy.
/// </summary>
/// <typeparam name="T">The entity type this specification applies to.</typeparam>
/// <remarks>
/// Specifications are composable: use <see cref="And"/>, <see cref="Or"/>,
/// and <see cref="Not"/> to build complex query criteria from smaller,
/// reusable pieces.
/// </remarks>
/// <example>
/// <code>
/// // Defining specifications:
/// public sealed class ActiveCustomerSpec : Specification&lt;Customer&gt;
/// {
///     public override Expression&lt;Func&lt;Customer, bool&gt;&gt; ToExpression()
///         => c => !c.IsDeleted &amp;&amp; c.Status == CustomerStatus.ActiveId;
/// }
///
/// public sealed class CustomerBySegmentSpec : Specification&lt;Customer&gt;
/// {
///     private readonly int _segmentId;
///     public CustomerBySegmentSpec(int segmentId) => _segmentId = segmentId;
///     public override Expression&lt;Func&lt;Customer, bool&gt;&gt; ToExpression()
///         => c => c.SegmentId == _segmentId;
/// }
///
/// // Composing and applying:
/// var spec = new ActiveCustomerSpec().And(new CustomerBySegmentSpec(5));
/// var results = await dbContext.Customers
///     .Where(spec.ToExpression())
///     .ToListAsync();
/// </code>
/// </example>
public abstract class Specification<T>
{
    /// <summary>Returns the predicate expression for use in EF Core LINQ queries.</summary>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Evaluates the specification against an in-memory entity instance.
    /// </summary>
    /// <param name="entity">The entity to test.</param>
    /// <returns><c>true</c> if the entity satisfies this specification.</returns>
    public bool IsSatisfiedBy(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return ToExpression().Compile()(entity);
    }

    /// <summary>
    /// Returns a new specification that is satisfied when both this specification
    /// AND <paramref name="other"/> are satisfied.
    /// </summary>
    public Specification<T> And(Specification<T> other) =>
        new AndSpecification<T>(this, other);

    /// <summary>
    /// Returns a new specification that is satisfied when either this specification
    /// OR <paramref name="other"/> is satisfied.
    /// </summary>
    public Specification<T> Or(Specification<T> other) =>
        new OrSpecification<T>(this, other);

    /// <summary>
    /// Returns a new specification that is satisfied when this specification
    /// is <b>not</b> satisfied.
    /// </summary>
    public Specification<T> Not() =>
        new NotSpecification<T>(this);
}
