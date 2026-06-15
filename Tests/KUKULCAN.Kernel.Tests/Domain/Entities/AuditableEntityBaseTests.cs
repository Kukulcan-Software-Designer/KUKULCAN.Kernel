using System;
using FluentAssertions;
using KUKULCAN.Kernel.Domain.Entities;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.Entities;

public class AuditableEntityBaseTests
{
    private sealed class TestAuditableEntity(Guid id) : AuditableEntityBase<Guid>(id);

    [Fact]
    public void SetCreated_AssignsCreatedMetadata()
    {
        var id = Guid.NewGuid();
        var entity = new TestAuditableEntity(id);

        DateTimeOffset now = DateTimeOffset.UtcNow;
        entity.SetCreated("creator", now);
        entity.CreatedBy.Should().Be("creator");
        entity.CreatedAt.Should().Be(now);
    }

            [Fact]
    public void SetUpdated_AssignsUpdatedMetadata()
    {
        var id = Guid.NewGuid();
        var entity = new TestAuditableEntity(id);

        DateTimeOffset now = DateTimeOffset.UtcNow;
        entity.SetUpdated("updater", now);
        entity.UpdatedBy.Should().Be("updater");
        entity.UpdatedAt.Should().Be(now);
    }
}
