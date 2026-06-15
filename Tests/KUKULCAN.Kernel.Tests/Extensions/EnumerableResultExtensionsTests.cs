using System.Collections.Generic;
using FluentAssertions;
using KUKULCAN.Kernel.Domain.Result;
using KUKULCAN.Kernel.Extensions;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Extensions;

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
