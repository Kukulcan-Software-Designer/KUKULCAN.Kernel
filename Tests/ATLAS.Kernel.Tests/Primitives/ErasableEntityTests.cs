using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Primitives;

namespace ATLAS.Kernel.Tests.Primitives;

public class ErasableEntityTests
{
    private class E : ErasableEntity { }

    [Fact]
    public void Erased_Default_IsFalse()
    {
        var e = new E();
        e.Erased.Should().BeFalse();
        e.Erased = true;
        e.Erased.Should().BeTrue();
    }
}
