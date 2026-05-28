using System.Collections.Generic;
using ATLAS.Kernel.Domain.Result;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Extensions;

namespace ATLAS.Kernel.Tests.Extensions;

public class EnumerableResultExtensionsTests
{
    [Fact]
    public void ToResult_ReturnsSuccessWithValue()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 };
        Result<IReadOnlyList<int>> result = list.ToResult();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(list);
    }
}
