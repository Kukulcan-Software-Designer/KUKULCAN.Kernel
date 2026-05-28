namespace ATLAS.Kernel.Domain.Result;

/// <summary>
/// Categorizes the type of error encapsulated in an <see cref="Error"/> value.
/// Used by API middleware to map domain errors to HTTP status codes.
/// </summary>
/// <example>
/// <code>
/// var httpStatus = error.Type switch
/// {
///     ErrorType.Validation  => StatusCodes.Status422UnprocessableEntity,
///     ErrorType.NotFound    => StatusCodes.Status404NotFound,
///     ErrorType.Conflict    => StatusCodes.Status409Conflict,
///     ErrorType.Forbidden   => StatusCodes.Status403Forbidden,
///     ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
///     _                     => StatusCodes.Status500InternalServerError
/// };
/// </code>
/// </example>
public enum ErrorType
{
    /// <summary>No error. Used by <see cref="Error.None"/>.</summary>
    None = 0,

    /// <summary>
    /// One or more input validation rules were violated.
    /// Maps to HTTP 422 Unprocessable Entity.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// The requested resource does not exist.
    /// Maps to HTTP 404 Not Found.
    /// </summary>
    NotFound = 2,

    /// <summary>
    /// The operation conflicts with the current state of the resource
    /// (e.g., duplicate key, optimistic concurrency violation).
    /// Maps to HTTP 409 Conflict.
    /// </summary>
    Conflict = 3,

    /// <summary>
    /// The authenticated user does not have permission to perform the operation.
    /// Maps to HTTP 403 Forbidden.
    /// </summary>
    Forbidden = 4,

    /// <summary>
    /// The request requires authentication.
    /// Maps to HTTP 401 Unauthorized.
    /// </summary>
    Unauthorized = 5,

    /// <summary>
    /// An unexpected internal error occurred.
    /// Maps to HTTP 500 Internal Server Error.
    /// </summary>
    Unexpected = 6,
}
