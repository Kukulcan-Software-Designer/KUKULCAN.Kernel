using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Extensions;

namespace ATLAS.Kernel.Tests.Extensions;

public class IListExtensionTests
{
    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_ForNullOrEmpty()
    {
        List<int>? n = null;
        n.IsNullOrEmpty().Should().BeTrue();
        var empty = new List<int>();
        empty.IsNullOrEmpty().Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsFalse_ForNonEmpty()
    {
        var list = new List<int> { 1 };
        list.IsNullOrEmpty().Should().BeFalse();
    }
}
