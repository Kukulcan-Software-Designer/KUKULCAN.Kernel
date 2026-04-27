namespace Atlas.SharedKernel.Infrastructure.Extensions;

/// <summary>
/// Extension methods for <see cref="decimal"/> used in financial and inventory calculations.
/// </summary>
/// <example>
/// <code>
/// 99.995m.RoundMidpoint(2)          // 100.00  (commercial rounding)
/// 25m.AsPercentageOf(200m)          // 12.5    (12.5%)
/// 1000m.ApplyPercentage(21m)        // 210.00  (VAT amount)
/// (-5m).Abs()                       // 5
/// 1234567.89m.ToThousands()         // "1,234,567.89"
/// 15m.AtLeast(10m)                  // 15
/// 15m.AtMost(10m)                   // 10
/// </code>
/// </example>
public static class DecimalExtensions
{
    /// <param name="value">The value to round.</param>
    extension(decimal value)
    {
        /// <summary>
        /// Rounds using <see cref="MidpointRounding.AwayFromZero"/> (commercial / financial rounding).
        /// </summary>
        /// <param name="decimals">Decimal places (default: 2).</param>
        public decimal RoundMidpoint(int decimals = 2) =>
            Math.Round(value, decimals, MidpointRounding.AwayFromZero);

        /// <summary>Rounds using <see cref="MidpointRounding.ToEven"/> (banker's rounding).</summary>
        public decimal RoundBankers(int decimals = 2) =>
            Math.Round(value, decimals, MidpointRounding.ToEven);

        /// <summary>Returns <c>true</c> when the value is strictly greater than zero.</summary>
        public bool IsPositive() => value > 0;

        /// <summary>Returns <c>true</c> when the value is strictly less than zero.</summary>
        public bool IsNegative() => value < 0;

        /// <summary>Returns <c>true</c> when the value equals zero exactly.</summary>
        public bool IsZero() => value == 0m;

        /// <summary>Returns the absolute value.</summary>
        public decimal Abs() => Math.Abs(value);

        /// <summary>
        /// Calculates the percentage this value represents of <paramref name="total"/>.
        /// Returns <c>0</c> when <paramref name="total"/> is zero (division guard).
        /// </summary>
        /// <example><code>25m.AsPercentageOf(200m) == 12.5m</code></example>
        public decimal AsPercentageOf(decimal total, int decimals = 4) =>
            total == 0 ? 0m
                : Math.Round(value / total * 100m, decimals, MidpointRounding.AwayFromZero);

        /// <summary>Applies a percentage (0–100) to this value and returns the calculated portion.</summary>
        /// <example><code>1000m.ApplyPercentage(21m) == 210.00m</code></example>
        public decimal ApplyPercentage(decimal percentage, int decimals = 6) =>
            Math.Round(value * (percentage / 100m), decimals, MidpointRounding.AwayFromZero);

        /// <summary>Formats with thousands separators using invariant culture.</summary>
        /// <example><code>1234567.89m.ToThousands() == "1,234,567.89"</code></example>
        public string ToThousands(int decimals = 2) =>
            value.ToString($"N{decimals}", System.Globalization.CultureInfo.InvariantCulture);

        /// <summary>Returns the larger of this value and <paramref name="min"/>.</summary>
        public decimal AtLeast(decimal min) => Math.Max(value, min);

        /// <summary>Returns the smaller of this value and <paramref name="max"/>.</summary>
        public decimal AtMost(decimal max) => Math.Min(value, max);
    }
}
