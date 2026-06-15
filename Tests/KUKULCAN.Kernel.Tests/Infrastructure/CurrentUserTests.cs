using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using KUKULCAN.Kernel.Abstractions.Interfaces.Infrastructure;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Infrastructure;

public class CurrentUserTests
{
    private sealed class FakeUser : ICurrentUser
    {
        public Guid UserId { get; } = Guid.NewGuid();
        public string UserName { get; } = "test";
        public string? Email { get; } = "a@b.com";
        public IReadOnlyList<string> Roles { get; } = new List<string>{"Admin","User"};
        public Guid TenantId { get; } = Guid.NewGuid();
        public bool IsAuthenticated { get; } = true;
        public bool IsInRole(string role) => Roles.Contains(role);
        public bool IsInAllRoles(params string[] roles) => roles.Length==0 || roles.All(r=>Roles.Contains(r));
    }

    [Fact]
    public void IsInRole_Works()
    {
        var u = new FakeUser();
        u.IsInRole("Admin").Should().BeTrue();
        u.IsInRole("Nope").Should().BeFalse();
    }

    [Fact]
    public void IsInAllRoles_Works()
    {
        var u = new FakeUser();
        u.IsInAllRoles("Admin","User").Should().BeTrue();
        u.IsInAllRoles("Admin","Nope").Should().BeFalse();
    }
}
