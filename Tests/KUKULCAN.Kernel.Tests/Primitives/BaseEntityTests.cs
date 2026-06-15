using FluentAssertions;
using KUKULCAN.Kernel.Primitives;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Primitives;

public class BaseEntityTests
{
    private class E : BaseEntity { }

    [Fact]
    public void Default_Id_IsZero()
    {
        var e = new E();
        e.Id.Should().Be(0);
        e.Id = 42;
        e.Id.Should().Be(42);
    }
}
