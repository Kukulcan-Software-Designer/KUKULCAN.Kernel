using System;
using FluentAssertions;
using Xunit;
using ATLAS.Kernel.Domain.Entities;

namespace ATLAS.Kernel.Tests.Domain.Entities;

public class TenantEntityBaseTests
{
    private sealed class TestTenantEntity : TenantEntityBase<Guid>
    {
        public TestTenantEntity(Guid id, Guid tenantId) : base(id)
        {
            TenantId = tenantId;
        }
    }

    [Fact]
    public void MarkAsDeleted_SetsFlagsAndMetadata()
    {
        var id = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var entity = new TestTenantEntity(id, tenant);

        entity.IsDeleted.Should().BeFalse();
        DateTimeOffset now = DateTimeOffset.UtcNow;
        entity.MarkAsDeleted("system", now);
        entity.IsDeleted.Should().BeTrue();
        entity.DeletedAt.Should().Be(now);
        entity.DeletedBy.Should().Be("system");
    }

    [Fact]
    public void MarkAsDeleted_ThrowsOnEmptyDeletedBy()
    {
        var id = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var entity = new TestTenantEntity(id, tenant);

        Action act = () => entity.MarkAsDeleted("   ", DateTimeOffset.UtcNow);
        act.Should().Throw<ArgumentException>().WithMessage("*DeletedBy must not be empty.*");
    }

    [Fact]
    public void Restore_ResetsDeletionState()
    {
        var id = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var entity = new TestTenantEntity(id, tenant);

        DateTimeOffset now = DateTimeOffset.UtcNow;
        entity.MarkAsDeleted("user", now);
        entity.Restore();
        entity.IsDeleted.Should().BeFalse();
        entity.DeletedAt.Should().BeNull();
        entity.DeletedBy.Should().BeNull();
    }
}
