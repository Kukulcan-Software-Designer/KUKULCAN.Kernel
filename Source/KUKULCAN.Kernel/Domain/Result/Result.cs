namespace KUKULCAN.Kernel.Domain.Result;

/// <summary>
/// Represents the outcome of a void command operation using the Railway-Oriented
/// Programming pattern. Encapsulates either a success state or a
/// failure <see cref="Error"/> without using exceptions for control flow.
/// </summary>
/// <remarks>
/// For operations that return a value on success, use <see cref="Result{T}"/>.
/// </remarks>
/// <example>
/// <code>
/// // Returning success from a command handler:
/// public async Task&lt;Result&gt; Handle(DeleteCustomerCommand cmd, CancellationToken ct)
/// {
///     var customer = await _repo.GetByIdAsync(cmd.CustomerId, ct);
///     if (customer is null)
///         return Error.NotFound("Customer.NotFound", $"Customer {cmd.CustomerId} not found.");
///
///     _repo.SoftDelete(customer);
///     await _unitOfWork.SaveChangesAsync(ct);
///     return Result.Ok();
/// }
///
/// // Handling the result in the API layer:
/// var result = await mediator.Send(command, ct);
/// if (result.IsFailure)
///     return BadRequest(result.Error);
/// return NoContent();
/// </code>
/// </example>
public sealed class Result
{
    private Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException("A successful result cannot carry an error.");
            case false when error == Error.None:
                throw new InvalidOperationException("A failed result must carry an error.");
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    /// <summary>Gets a value indicating whether the operation succeeded.</summary>
    public bool IsSuccess { get; }

    /// <summary>Gets a value indicating whether the operation failed.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error when <see cref="IsFailure"/> is <c>true</c>, or
    /// <see cref="Error.None"/> when the operation succeeded.
    /// </summary>
    public Error Error { get; }

    // ── Factory methods ──────────────────────────────────────────────────────

    /// <summary>Creates a successful result.</summary>
    public static Result Ok() => new(true, Error.None);

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error describing why the operation failed.</param>
    public static Result Fail(Error error) => new(false, error);

    // ── Implicit conversions ─────────────────────────────────────────────────

    /// <summary>
    /// Implicitly converts an <see cref="Error"/> to a failed <see cref="Result"/>,
    /// enabling concise failure returns without calling <see cref="Fail"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// return Error.NotFound("Customer.NotFound", "Customer not found.");
    /// // is equivalent to:
    /// return Result.Fail(Error.NotFound("Customer.NotFound", "Customer not found."));
    /// </code>
    /// </example>
    public static implicit operator Result(Error error) => Fail(error);

    // ── Railway methods ──────────────────────────────────────────────────────

    /// <summary>
    /// Executes <paramref name="onSuccess"/> if the result is successful;
    /// otherwise returns this result unchanged.
    /// </summary>
    public Result OnSuccess(Action onSuccess)
    {
        if (IsSuccess) onSuccess();
        return this;
    }

    /// <summary>
    /// Executes <paramref name="onFailure"/> if the result is a failure;
    /// otherwise returns this result unchanged.
    /// </summary>
    public Result OnFailure(Action<Error> onFailure)
    {
        if (IsFailure) onFailure(Error);
        return this;
    }

    /// <summary>
    /// Projects the result to a value of type <typeparamref name="T"/> by
    /// executing either <paramref name="onSuccess"/> or <paramref name="onFailure"/>.
    /// </summary>
    public T Match<T>(Func<T> onSuccess, Func<Error, T> onFailure) =>
        IsSuccess ? onSuccess() : onFailure(Error);

    /// <inheritdoc/>
    public override string ToString() =>
        IsSuccess ? "Result { IsSuccess = true }" : $"Result {{ IsFailure = true, Error = {Error} }}";
}
