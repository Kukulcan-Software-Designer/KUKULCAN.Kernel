namespace Atlas.SharedKernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Defines the contract for aggregate roots that collect domain events during
/// a unit-of-work and dispatch them after a successful <c>SaveChangesAsync</c>.
/// </summary>
/// <remarks>
/// Domain events are added to the internal collection during aggregate operations
/// and are cleared immediately after dispatch by the
/// <c>DomainEventDispatchInterceptor</c> in <c>Atlas.Database</c>.
/// </remarks>
/// <example>
/// <code>
/// // Inside an aggregate root method:
/// public void Activate()
/// {
///     if (IsActive) return;
///     IsActive = true;
///     AddDomainEvent(new CustomerActivatedDomainEvent(Id));
/// }
///
/// // After SaveChangesAsync, the interceptor calls:
/// foreach (var evt in aggregate.DomainEvents)
///     await publisher.Publish(evt, ct);
/// aggregate.ClearDomainEvents();
/// </code>
/// </example>
public interface IDomainEventHolder
{
    /// <summary>
    /// Gets the domain events that have been raised but not yet dispatched.
    /// </summary>
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>Adds a domain event to the pending dispatch queue.</summary>
    /// <param name="domainEvent">The domain event to enqueue. Must not be null.</param>
    void AddDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Removes all pending domain events after they have been dispatched.
    /// Called automatically by the <c>DomainEventDispatchInterceptor</c>.
    /// </summary>
    void ClearDomainEvents();
}
