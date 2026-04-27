namespace Atlas.SharedKernel.Domain.Events;

/// <summary>
/// Abstract base class for all integration events in ATLAS.
/// Integration events cross bounded-context boundaries via the message bus
/// (RabbitMQ / Azure Service Bus) and enable asynchronous, decoupled
/// inter-module communication.
/// </summary>
/// <remarks>
/// <para>
/// Integration events must be <b>idempotent</b> — the consuming module must
/// handle duplicate deliveries (at-least-once delivery guarantee) without
/// producing incorrect side effects.
/// </para>
/// <para>
/// Use <c>sealed record</c> syntax for concrete integration events to benefit
/// from automatic equality and immutability.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Defining an integration event:
/// public sealed record CustomerCreatedIntegrationEvent(
///     Guid CustomerId,
///     string CustomerCode,
///     string CompanyName,
///     Guid TenantId,
///     DateTimeOffset CreatedAt) : IntegrationEvent;
///
/// // Publishing (typically in a domain event handler):
/// await _eventPublisher.PublishAsync(
///     new CustomerCreatedIntegrationEvent(
///         customer.Id,
///         customer.Code,
///         customer.CompanyName,
///         _tenantContext.TenantId,
///         _dateTimeProvider.UtcNow),
///     cancellationToken);
/// </code>
/// </example>
public abstract record IntegrationEvent : IIntegrationEvent
{
    /// <inheritdoc/>
    public Guid EventId { get; } = Guid.NewGuid();

    /// <inheritdoc/>
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;

    /// <inheritdoc/>
    public string EventType => GetType().FullName
        ?? GetType().Name;
}
