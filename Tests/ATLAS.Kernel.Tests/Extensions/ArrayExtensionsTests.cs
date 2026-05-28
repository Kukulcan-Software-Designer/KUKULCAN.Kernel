using System;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Extensions;

namespace ATLAS.Kernel.Tests.Extensions;

public class ArrayExtensionsTests
{
    [Fact]
    public void Add_AppendsItem_WhenPrependFalse()
    {
        int[] src = [1, 2];
        int[] result = src.Add(3);
        result.Should().Equal(new[] { 1, 2, 3 });
    }

    [Fact]
    public void Add_PrependsItem_WhenPrependTrue()
    {
        int[] src = [1, 2];
        int[] result = src.Add(0, prepend: true);
        result.Should().Equal(new[] { 0, 1, 2 });
    }

    [Fact]
    public void Add_ThrowsOnNullSource()
    {
        int[]? src = null;
        Action act = () => src!.Add(1);
        act.Should().Throw<ArgumentNullException>();
    }
}
