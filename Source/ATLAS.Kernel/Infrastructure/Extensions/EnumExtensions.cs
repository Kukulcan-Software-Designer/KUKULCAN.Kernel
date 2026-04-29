using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ATLAS.Kernel.Infrastructure.Extensions;

/// <summary>
/// Extension methods for <see cref="Enum"/> values.
/// </summary>
/// <example>
/// <code>
/// public enum OrderStatus
/// {
///     [Display(Name = "Pending Confirmation")] Pending = 1,
///     [Description("Order has been shipped")]  Shipped = 2,
/// }
///
/// OrderStatus.Pending.GetDisplayName()                       // "Pending Confirmation"
/// OrderStatus.Shipped.GetDescription()                       // "Order has been shipped"
/// EnumExtensions.GetAll&lt;OrderStatus&gt;()                 // [Pending, Shipped]
/// EnumExtensions.ParseIgnoreCase&lt;OrderStatus&gt;("shipped") // OrderStatus.Shipped
/// </code>
/// </example>
public static class EnumExtensions
{
    extension(Enum value)
    {
        /// <summary>
        /// Returns the <see cref="DisplayAttribute.Name"/> of the enum member,
        /// or the member name when no attribute is present.
        /// </summary>
        public string GetDisplayName()
        {
            var attr = value.GetType()
                .GetMember(value.ToString()).FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>();
            return attr?.Name ?? value.ToString();
        }

        /// <summary>
        /// Returns the <see cref="DescriptionAttribute"/> of the enum member,
        /// or the member name when no attribute is present.
        /// </summary>
        public string GetDescription()
        {
            var attr = value.GetType()
                .GetMember(value.ToString()).FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }

    /// <summary>Returns all defined values of <typeparamref name="T"/>.</summary>
    public static IReadOnlyList<T> GetAll<T>()
        where T : struct, Enum => Enum.GetValues<T>();

    /// <summary>
    /// Parses a string to <typeparamref name="T"/>, ignoring case.
    /// </summary>
    /// <exception cref="ArgumentException">When the value is not a valid member name.</exception>
    public static T ParseIgnoreCase<T>(string value) where T : struct, Enum
    {
        if (!Enum.TryParse<T>(value, ignoreCase: true, out var result))
            throw new ArgumentException(
                $"'{value}' is not a valid member of '{typeof(T).Name}'. " +
                $"Valid values: {string.Join(", ", Enum.GetNames<T>())}");
        return result;
    }

    /// <summary>
    /// Attempts to parse a string to <typeparamref name="T"/>, ignoring case.
    /// Returns <c>null</c> on failure.
    /// </summary>
    public static T? TryParseIgnoreCase<T>(string? value) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return Enum.TryParse<T>(value, ignoreCase: true, out var r) ? r : null;
    }
}
