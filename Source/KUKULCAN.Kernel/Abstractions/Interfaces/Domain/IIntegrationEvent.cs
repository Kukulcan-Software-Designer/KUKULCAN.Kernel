namespace KUKULCAN.Kernel.Abstractions.Interfaces.Domain;

/// <summary>
/// Defines the contract for integration events that cross bounded-context boundaries
/// via the message bus (RabbitMQ / Azure Service Bus).
/// </summary>
/// <remarks>
/// Integration events are published asynchronously after a successful unit-of-work
/// commit and are consumed by other modules through the message bus.
/// They must be idempotent — consumers must handle duplicate deliveries gracefully.
/// </remarks>
/// <example>
/// <code>
/// public sealed record CustomerCreatedIntegrationEvent(
///     Guid CustomerId,
///     string CustomerCode,
///     Guid TenantId) : IntegrationEvent;
///
/// // Publishing:
/// await eventPublisher.PublishAsync(new CustomerCreatedIntegrationEvent(
///     customer.Id, customer.Code, tenantContext.TenantId), cancellationToken);
/// </code>
/// </example>
public interface IIntegrationEvent
{
    /// <summary>Gets the globally unique identifier of this event instance.</summary>
    Guid EventId { get; }

    /// <summary>Gets the UTC timestamp at which this event was created.</summary>
    DateTimeOffset OccurredAt { get; }

    /// <summary>
    /// Gets the fully qualified event type name used for routing on the message bus
    /// (e.g., <c>"ATLAS.CRM.CustomerCreatedIntegrationEvent"</c>).
    /// </summary>
    string EventType { get; }
}
