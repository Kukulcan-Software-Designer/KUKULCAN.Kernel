using System;
using FluentAssertions;
using KUKULCAN.Kernel.Infrastructure.Primitives;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Infrastructure.Primitives;

public class SequentialGuidTests
{
    [Fact]
    public void NewSequentialGuid_ReturnsGuid()
    {
        Guid g1 = SequentialGuid.NewSequentialGuid();
        Guid g2 = SequentialGuid.NewSequentialGuid();
        g1.Should().NotBe(Guid.Empty);
        g2.Should().NotBe(Guid.Empty);
        g1.Should().NotBe(g2);
    }

    [Fact]
    public void NewSequentialGuidAtEnd_ReturnsGuid()
    {
        Guid g1 = SequentialGuid.NewSequentialGuidAtEnd();
        Guid g2 = SequentialGuid.NewSequentialGuidAtEnd();
        g1.Should().NotBe(Guid.Empty);
        g2.Should().NotBe(Guid.Empty);
        g1.Should().NotBe(g2);
    }
}
