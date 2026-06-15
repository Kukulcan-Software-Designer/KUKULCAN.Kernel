namespace KUKULCAN.Kernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Defines the contract for domain events raised within a bounded context.
/// Domain events are published in-process via MediatR's <c>IPublisher</c>
/// immediately after a successful <c>SaveChangesAsync</c> call.
/// </summary>
/// <remarks>
/// Domain events differ from integration events in that they are always
/// handled within the same bounded context and the same transaction boundary.
/// For cross-module communication, use <see cref="IIntegrationEvent"/> instead.
/// </remarks>
/// <example>
/// <code>
/// // Handling a domain event:
/// public class CustomerCreatedDomainEventHandler
///     : INotificationHandler&lt;CustomerCreatedDomainEvent&gt;
/// {
///     public Task Handle(CustomerCreatedDomainEvent notification, CancellationToken ct)
///     {
///         // React to the event within the same bounded context
///         return Task.CompletedTask;
///     }
/// }
/// </code>
/// </example>
public interface IDomainEvent
{
    /// <summary>Gets the globally unique identifier of this event instance.</summary>
    Guid EventId { get; }

    /// <summary>Gets the UTC timestamp at which this event occurred.</summary>
    DateTimeOffset OccurredAt { get; }
}
