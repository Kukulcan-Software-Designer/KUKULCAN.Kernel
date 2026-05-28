using System.Collections.Generic;
using ATLAS.Kernel.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ATLAS.Kernel.Tests.Domain.ValueObjects;

public class ValueObjectTests
{
    private sealed class MyVo(int x) : ValueObject
    {
        private int X { get; } = x;
        protected override IEnumerable<object?> GetEqualityComponents() { yield return X; }
    }

    private sealed class OtherVo(int x) : ValueObject
    {
        private int X { get; } = x;
        protected override IEnumerable<object?> GetEqualityComponents() { yield return X; }
    }

    [Fact]
    public void EqualValueObjects_AreEqual()
    {
        var a = new MyVo(1);
        var b = new MyVo(1);
        a.Equals(b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
    }

    [Fact]
    public void DifferentValuesDifferentTypesAndNull_AreNotEqual()
    {
        var a = new MyVo(1);

        a.Equals(new MyVo(2)).Should().BeFalse();
        a.Equals(new OtherVo(1)).Should().BeFalse();
        a.Equals(null).Should().BeFalse();
        (a == null).Should().BeFalse();
        (a != new MyVo(2)).Should().BeTrue();
    }
}
