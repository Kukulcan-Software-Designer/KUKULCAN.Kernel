using KUKULCAN.Kernel.Abstractions.Interfaces.Domain;

namespace KUKULCAN.Kernel.Domain.Events;

/// <summary>
/// Abstract base class for all domain events in KUKULCAN applications.
/// Domain events are raised within an aggregate root to signal that something
/// meaningful has occurred in the domain. They are dispatched in-process via
/// MediatR's <c>IPublisher</c> immediately after <c>SaveChangesAsync</c> succeeds.
/// </summary>
/// <remarks>
/// <para>
/// Derive from this class (rather than implementing <see cref="IDomainEvent"/> directly)
/// to ensure consistent <see cref="EventId"/> and <see cref="OccurredAt"/> values.
/// </para>
/// <para>
/// Domain events are handled within the same bounded context. For cross-module
/// communication, convert them to integration events inside a domain event handler.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Defining a domain event:
/// public sealed record CustomerActivatedDomainEvent(Guid CustomerId)
///     : DomainEvent;
///
/// // Raising from an aggregate root:
/// public void Activate()
/// {
///     if (IsActive) return;
///     IsActive = true;
///     AddDomainEvent(new CustomerActivatedDomainEvent(Id));
/// }
///
/// // Handling:
/// public sealed class CustomerActivatedDomainEventHandler
///     : INotificationHandler&lt;CustomerActivatedDomainEvent&gt;
/// {
///     public Task Handle(CustomerActivatedDomainEvent notification, CancellationToken ct)
///     {
///         // e.g., publish integration event, update projection
///         return Task.CompletedTask;
///     }
/// }
/// </code>
/// </example>
public abstract record DomainEvent : IDomainEvent
{
    /// <inheritdoc/>
    public Guid EventId { get; } = Guid.NewGuid();

    /// <inheritdoc/>
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
