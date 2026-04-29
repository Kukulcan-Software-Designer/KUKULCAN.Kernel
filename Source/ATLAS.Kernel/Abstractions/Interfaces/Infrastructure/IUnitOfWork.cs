namespace ATLAS.Kernel.Abstractions.Interfaces.Infrastructure;

/// <summary>
/// Defines the unit-of-work pattern that groups multiple repository operations
/// into a single atomic transaction.
/// </summary>
/// <remarks>
/// <para>
/// In ATLAS, the <c>TransactionBehavior</c> MediatR pipeline behavior wraps every
/// <c>ICommand</c> in a unit-of-work transaction automatically, so explicit
/// transaction management is rarely needed in application code.
/// </para>
/// <para>
/// The concrete implementation per module is registered as
/// <c>UnitOfWork&lt;TDbContext&gt;</c> in each module's DI registration via
/// <c>AddAtlasDbContext&lt;TContext&gt;()</c>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Explicit transaction — only needed for cross-repository operations
/// // outside the MediatR pipeline:
/// await unitOfWork.BeginTransactionAsync(ct);
/// try
/// {
///     await orderRepo.AddAsync(order, ct);
///     inventoryRepo.SoftDelete(reservedStock);
///     await unitOfWork.SaveChangesAsync(ct);
///     await unitOfWork.CommitTransactionAsync(ct);
/// }
/// catch
/// {
///     await unitOfWork.RollbackTransactionAsync(ct);
///     throw;
/// }
/// </code>
/// </example>
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Persists all pending changes tracked by the underlying <c>DbContext</c>
    /// to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>Begins a new explicit database transaction.</summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>Commits the current explicit transaction.</summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>Rolls back the current explicit transaction, discarding all pending changes.</summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
