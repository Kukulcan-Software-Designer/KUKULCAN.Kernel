using System;
using FluentAssertions;
using KUKULCAN.Kernel.Domain.Guards;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.Guards;

public class GuardTests
{
    [Fact]
    public void NullOrWhiteSpace_ThrowsForEmpty()
    {
        Action act = () => Guard.Against.NullOrWhiteSpace(null, nameof(Guard));
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Default_ThrowsForDefaultGuid()
    {
        Action act = () => Guard.Against.Default(Guid.Empty, "id");
        act.Should().Throw<ArgumentException>();
    }
}
