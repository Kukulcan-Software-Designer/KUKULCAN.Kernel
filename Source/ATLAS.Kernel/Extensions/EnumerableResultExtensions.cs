namespace ATLAS.Kernel.Extensions;

/// <summary>
/// Extension methods for converting an <see cref="IReadOnlyList{T}"/> to a <see cref="Result{T}"/>.
/// </summary>
public static class EnumerableResultExtensions
{
    /// <summary>
    /// Converts an <see cref="IReadOnlyList{T}"/> to a successful <c>Result&lt;IReadOnlyList&lt;T&gt;&gt;</c> containing the provided list.
    /// </summary>
    /// <param name="list">The list to convert into a successful Result containing the list.</param>
    /// <typeparam name="T">The element type of the list.</typeparam>
    /// <returns>A successful <c>Result&lt;IReadOnlyList&lt;T&gt;&gt;</c> containing the provided list.</returns>
    public static Result<IReadOnlyList<T>> ToResult<T>(this IReadOnlyList<T> list) => Result<IReadOnlyList<T>>.Ok(list);
}
