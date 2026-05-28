using ATLAS.Kernel.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ATLAS.Kernel.Tests.Domain.Entities;

public class ReferenceEntityTests
{
    private sealed class R(int id) : ReferenceEntity<int>(id);

    [Fact]
    public void ActivateDeactivate_Work()
    {
        var r = new R(1);

        r.IsActive.Should().BeTrue();
        r.Deactivate();
        r.IsActive.Should().BeFalse();
        r.Activate();
        r.IsActive.Should().BeTrue();
    }

    [Fact]
    public void ActivateDeactivate_AreIdempotent()
    {
        var r = new R(1);

        r.Activate();
        r.IsActive.Should().BeTrue();
        r.Deactivate();
        r.Deactivate();
        r.IsActive.Should().BeFalse();
    }
}
