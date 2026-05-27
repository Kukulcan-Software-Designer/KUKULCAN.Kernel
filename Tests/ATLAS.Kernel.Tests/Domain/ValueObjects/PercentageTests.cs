using ATLAS.Kernel.Domain.Result;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Domain.ValueObjects;

namespace ATLAS.Kernel.Tests.Domain.ValueObjects;

public class PercentageTests
{
    [Fact]
    public void CreateAndApply_Works()
    {
        Result<Percentage> r = Percentage.Create(21m);
        r.IsSuccess.Should().BeTrue();
        Percentage p = r.Value;
        p.AsDecimalFraction.Should().Be(0.21m);
        p.ApplyTo(1000m).Should().Be(210m);
        p.ApplyDiscount(1000m).Should().Be(790m);
        p.ToString().Should().Be("21%");
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(100.01)]
    public void Create_ReturnsFailure_WhenOutOfRange(decimal value)
    {
        Result<Percentage> result = Percentage.Create(value);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Percentage.OutOfRange");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(100, 100)]
    public void Create_AllowsRangeBoundaries(decimal value, decimal expected)
    {
        Percentage.Create(value).Value.Value.Should().Be(expected);
    }
}
