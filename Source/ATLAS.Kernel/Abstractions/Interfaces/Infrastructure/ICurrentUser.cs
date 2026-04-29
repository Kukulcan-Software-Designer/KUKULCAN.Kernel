namespace ATLAS.Kernel.Abstractions.Interfaces.Infrastructure;

/// <summary>
/// Provides access to the identity and authorisation context of the authenticated
/// principal executing the current request.
/// </summary>
/// <remarks>
/// The implementation is resolved from the HTTP context (or background job context)
/// and registered with a per-request lifetime in the DI container.
/// Inject this interface into MediatR handlers, domain services, and interceptors
/// that require knowledge of the acting user.
/// </remarks>
/// <example>
/// <code>
/// // In a command handler:
/// public sealed class CreateOrderCommandHandler : IRequestHandler&lt;CreateOrderCommand, Result&lt;Guid&gt;&gt;
/// {
///     private readonly ICurrentUser _currentUser;
///
///     public async Task&lt;Result&lt;Guid&gt;&gt; Handle(CreateOrderCommand cmd, CancellationToken ct)
///     {
///         if (!_currentUser.IsInRole("SOM.Operator"))
///             return Error.Forbidden("Order.Create.Forbidden", "Insufficient permissions.");
///
///         var order = Order.Create(_currentUser.UserId, cmd.CustomerId, ...);
///         // ...
///     }
/// }
/// </code>
/// </example>
public interface ICurrentUser
{
    /// <summary>Gets the unique identifier of the authenticated user.</summary>
    Guid UserId { get; }

    /// <summary>Gets the username of the authenticated principal.</summary>
    string UserName { get; }

    /// <summary>Gets the email address of the authenticated user, if available.</summary>
    string? Email { get; }

    /// <summary>Gets the roles assigned to the authenticated user.</summary>
    IReadOnlyList<string> Roles { get; }

    /// <summary>Gets the identifier of the tenant the authenticated user belongs to.</summary>
    Guid TenantId { get; }

    /// <summary>Gets a value indicating whether a principal is currently authenticated.</summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Determines whether the authenticated user has been assigned the specified role.
    /// </summary>
    /// <param name="role">
    /// The role name to check (e.g., <c>"CRM.Admin"</c>, <c>"SOM.Operator"</c>).
    /// </param>
    bool IsInRole(string role);

    /// <summary>
    /// Determines whether the authenticated user has been assigned all the specified roles.
    /// </summary>
    bool IsInAllRoles(params string[] roles);
}
