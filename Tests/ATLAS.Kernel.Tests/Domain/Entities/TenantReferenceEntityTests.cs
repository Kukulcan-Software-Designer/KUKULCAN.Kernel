using ATLAS.Kernel.Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace ATLAS.Kernel.Tests.Domain.Entities;

public class TenantReferenceEntityTests
{
    private sealed class R : TenantReferenceEntity<int>
    {
        public R(int id, Guid tenantId, bool system) : base(id)
        {
            TenantId = tenantId;
            IsSystemDefined = system;
        }
    }

    [Fact]
    public void Deactivate_Throws_ForSystemDefined()
    {
        var r = new R(1, Guid.NewGuid(), true);

        Action act = r.Deactivate;

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Deactivate_Works_ForCustom()
    {
        var r = new R(1, Guid.NewGuid(), false);

        r.Deactivate();
        r.IsActive.Should().BeFalse();
        r.Activate();
        r.IsActive.Should().BeTrue();
    }

    [Fact]
    public void ActivateDeactivate_AreIdempotent_ForCustomReference()
    {
        var r = new R(1, Guid.NewGuid(), false);

        r.Activate();
        r.IsActive.Should().BeTrue();
        r.Deactivate();
        r.Deactivate();
        r.IsActive.Should().BeFalse();
    }
}
