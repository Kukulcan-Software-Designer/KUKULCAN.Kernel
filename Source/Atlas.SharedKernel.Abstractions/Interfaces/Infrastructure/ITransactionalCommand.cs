namespace Atlas.SharedKernel.Abstractions.Interfaces.Infrastructure;

/// <summary>
/// Marker interface that instructs the <c>TransactionBehavior</c> MediatR
/// pipeline behavior to wrap this command in a database transaction.
/// </summary>
/// <remarks>
/// Commands that modify multiple aggregates or raise integration events should
/// implement this marker to guarantee atomicity. Commands that only modify a
/// single aggregate typically do not require an explicit transaction.
/// </remarks>
/// <example>
/// <code>
/// public sealed record CreateSalesOrderCommand(
///     Guid CustomerId,
///     IReadOnlyList&lt;OrderLineDto&gt; Lines) : IRequest&lt;Result&lt;Guid&gt;&gt;, ITransactionalCommand;
/// </code>
/// </example>
public interface ITransactionalCommand { }
