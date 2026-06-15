namespace KUKULCAN.Kernel.Abstractions.Interfaces.Infrastructure;

/// <summary>
/// Abstracts the system clock to enable deterministic testing of time-dependent logic.
/// Inject this interface wherever the current date or time is required instead of
/// using <c>DateTimeOffset.UtcNow</c> or <c>DateTime.UtcNow</c> directly.
/// </summary>
/// <remarks>
/// Register a <c>SystemDateTimeProvider</c> (wrapping the real clock) in production
/// and a <c>FakeDateTimeProvider</c> (with a fixed time) in unit tests.
/// </remarks>
/// <example>
/// <code>
/// // Production registration:
/// services.AddSingleton&lt;IDateTimeProvider, SystemDateTimeProvider&gt;();
///
/// // In a test:
/// var fakeProvider = new FakeDateTimeProvider(
///     new DateTimeOffset(2026, 3, 1, 10, 0, 0, TimeSpan.Zero));
/// var handler = new CreateInvoiceCommandHandler(fakeProvider, ...);
/// </code>
/// </example>
public interface IDateTimeProvider
{
    /// <summary>Gets the current UTC date and time.</summary>
    DateTimeOffset UtcNow { get; }

    /// <summary>Gets today's UTC date (time component is midnight).</summary>
    DateOnly Today { get; }

    /// <summary>Gets the current Unix timestamp in seconds (epoch).</summary>
    long UnixTimestampSeconds { get; }
}
