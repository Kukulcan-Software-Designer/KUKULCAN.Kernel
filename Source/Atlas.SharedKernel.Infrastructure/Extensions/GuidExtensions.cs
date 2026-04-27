using Atlas.SharedKernel.Infrastructure.Primitives;

namespace Atlas.SharedKernel.Infrastructure.Extensions;

/// <summary>
/// Extension methods for <see cref="Guid"/> values.
/// </summary>
/// <example>
/// <code>
/// Guid.Empty.IsEmpty()                    // true
/// Guid.NewGuid().HasValue()               // true
/// Guid.NewGuid().ToShortString()          // e.g. "a3f2c1b4"  (first 8 hex chars)
/// GuidExtensions.NewSequential()          // sequential GUID for SQL Server
/// GuidExtensions.NewSequentialAtEnd()     // sequential GUID for PostgreSQL / MySQL
/// </code>
/// </example>
public static class GuidExtensions
{
    extension(Guid value)
    {
        /// <summary>Returns <c>true</c> when the GUID equals <see cref="Guid.Empty"/>.</summary>
        public bool IsEmpty() => value == Guid.Empty;

        /// <summary>Returns <c>true</c> when the GUID is not <see cref="Guid.Empty"/>.</summary>
        public bool HasValue() => value != Guid.Empty;

        /// <summary>
        /// Returns the first 8 hexadecimal characters of the GUID (no hyphens).
        /// Useful as a short correlation token in logs.
        /// </summary>
        public string ToShortString() =>
            value.ToString("N")[..8];
    }

    /// <summary>
    /// Creates a sequential GUID optimised for SQL Server clustered indexes.
    /// Delegates to <see cref="SequentialGuid.NewSequentialGuid"/>.
    /// </summary>
    public static Guid NewSequential() => SequentialGuid.NewSequentialGuid();

    /// <summary>
    /// Creates a sequential GUID optimised for PostgreSQL, MySQL, MariaDB, and Oracle.
    /// Delegates to <see cref="SequentialGuid.NewSequentialGuidAtEnd"/>.
    /// </summary>
    public static Guid NewSequentialAtEnd() => SequentialGuid.NewSequentialGuidAtEnd();
}
