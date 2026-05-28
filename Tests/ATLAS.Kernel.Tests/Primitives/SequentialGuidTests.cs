using System;
using FluentAssertions;
using Xunit;
using ATLAS.Kernel.Infrastructure.Primitives;

namespace ATLAS.Kernel.Tests.Primitives;

public class SequentialGuidTests
{
    [Fact]
    public void NewSequentialGuid_IsNonEmpty_AndDifferent()
    {
        Guid a = SequentialGuid.NewSequentialGuid();
        Guid b = SequentialGuid.NewSequentialGuid();
        a.Should().NotBe(Guid.Empty);
        b.Should().NotBe(Guid.Empty);
        a.Should().NotBe(b);
    }

    [Fact]
    public void NewSequentialGuidAtEnd_IsNonEmpty_AndDifferent()
    {
        Guid a = SequentialGuid.NewSequentialGuidAtEnd();
        Guid b = SequentialGuid.NewSequentialGuidAtEnd();
        a.Should().NotBe(Guid.Empty);
        b.Should().NotBe(Guid.Empty);
        a.Should().NotBe(b);
    }
}
