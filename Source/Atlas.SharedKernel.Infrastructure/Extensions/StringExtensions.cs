namespace Atlas.SharedKernel.Infrastructure.Extensions;

/// <summary>
/// Extension methods for <see cref="string"/> values commonly used across ATLAS modules.
/// </summary>
/// <example>
/// <code>
/// "Hello World!".ToSlug()          // "hello-world"
/// "hello world".ToTitleCase()      // "Hello World"
/// "CustomerStatus".ToSnakeCase()   // "customer_status"
/// "customer_status".ToCamelCase()  // "customerStatus"
/// "4111111111111111".Mask(4)       // "************1111"
/// "Very long text".Truncate(8)     // "Very lon..."
/// ((string?)null).IsNullOrWhiteSpace() // true
/// "hello".ToBase64()               // "aGVsbG8="
/// </code>
/// </example>
public static class StringExtensions
{
    extension(string value)
    {
        /// <summary>
        /// Returns a URL-friendly slug: lowercase, spaces and non-alphanumeric characters
        /// replaced with hyphens, consecutive hyphens collapsed.
        /// </summary>
        public string ToSlug()
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            var slug = value.Trim().ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", string.Empty);
            slug = Regex.Replace(slug, @"[\s-]+", "-");
            return slug.Trim('-');
        }

        /// <summary>
        /// Truncates to at most <paramref name="maxLength"/> characters, appending
        /// <paramref name="suffix"/> when truncation occurs.
        /// </summary>
        public string Truncate(int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength) return value;
            if (maxLength <= suffix.Length) return suffix[..maxLength];
            return string.Concat(value.AsSpan(0, maxLength - suffix.Length), suffix);
        }

        /// <summary>Converts to Title Case using invariant culture.</summary>
        public string ToTitleCase()
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo
                .ToTitleCase(value.ToLowerInvariant());
        }

        /// <summary>Converts PascalCase or camelCase to snake_case.</summary>
        /// <example><code>"CustomerStatus".ToSnakeCase() == "customer_status"</code></example>
        public string ToSnakeCase()
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            return Regex.Replace(value, @"([a-z0-9])([A-Z])", "$1_$2").ToLowerInvariant();
        }

        /// <summary>Converts snake_case or PascalCase to camelCase.</summary>
        /// <example><code>"customer_status".ToCamelCase() == "customerStatus"</code></example>
        public string ToCamelCase()
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            if (value.Contains('_'))
            {
                var parts = value.Split('_', StringSplitOptions.RemoveEmptyEntries);
                return parts[0].ToLowerInvariant() +
                       string.Concat(parts.Skip(1).Select(p => p.ToTitleCase()));
            }
            return char.ToLowerInvariant(value[0]) + value[1..];
        }

        /// <summary>
        /// Masks all but the last <paramref name="visibleChars"/> characters using
        /// <paramref name="maskChar"/>.
        /// </summary>
        /// <example><code>"4111111111111111".Mask(4) == "************1111"</code></example>
        public string Mask(int visibleChars = 4, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length <= visibleChars) return new string(maskChar, value.Length);
            return new string(maskChar, value.Length - visibleChars) + value[^visibleChars..];
        }
    }

    /// <summary>Returns <c>true</c> when the string is <c>null</c>, empty, or whitespace.</summary>
    public static bool IsNullOrWhiteSpace(this string? value) =>
        string.IsNullOrWhiteSpace(value);

    /// <summary>Returns <c>true</c> when the string has a non-whitespace value.</summary>
    public static bool HasValue(this string? value) =>
        !string.IsNullOrWhiteSpace(value);

    /// <summary>Encodes the string to Base64 (UTF-8).</summary>
    public static string ToBase64(this string value) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

    /// <summary>Decodes a Base64 UTF-8 string.</summary>
    public static string FromBase64(this string value) =>
        Encoding.UTF8.GetString(Convert.FromBase64String(value));

    /// <summary>Returns <paramref name="fallback"/> when the string is null or whitespace.</summary>
    public static string OrDefault(this string? value, string fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : value;
}
