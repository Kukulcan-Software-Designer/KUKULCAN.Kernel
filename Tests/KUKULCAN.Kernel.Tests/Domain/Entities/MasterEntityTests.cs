using FluentAssertions;
using KUKULCAN.Kernel.Domain.Entities;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.Entities;

public class MasterEntityTests
{
    private sealed class M(int id) : MasterEntity<int>(id);

    [Fact]
    public void ActivateDeactivate_Work()
    {
        var m = new M(1);

        m.IsActive.Should().BeTrue();
        m.Deactivate();
        m.IsActive.Should().BeFalse();
        m.Activate();
        m.IsActive.Should().BeTrue();
    }

    [Fact]
    public void ActivateDeactivate_AreIdempotent()
    {
        var m = new M(1);

        m.Activate();
        m.IsActive.Should().BeTrue();
        m.Deactivate();
        m.Deactivate();
        m.IsActive.Should().BeFalse();
    }
}
