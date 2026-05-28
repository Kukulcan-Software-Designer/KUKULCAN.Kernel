using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Infrastructure.Extensions;

namespace ATLAS.Kernel.Tests.Infrastructure.Extensions;

public class DecimalExtensionsTests
{
    [Fact]
    public void RoundMidpoint_Works()
    {
        (99.995m).RoundMidpoint().Should().Be(100.00m);
        (2.5m).RoundBankers(0).Should().Be(2m);
    }

    [Fact]
    public void ApplyPercentage_And_AsPercentageOf_Work()
    {
        1000m.ApplyPercentage(21m).Should().Be(210m);
        25m.AsPercentageOf(200m).Should().Be(12.5m);
        25m.AsPercentageOf(0m).Should().Be(0m);
    }

    [Fact]
    public void Sign_Abs_Limits_AndFormatting_Work()
    {
        1m.IsPositive().Should().BeTrue();
        (-1m).IsNegative().Should().BeTrue();
        0m.IsZero().Should().BeTrue();
        (-5m).Abs().Should().Be(5m);
        5m.AtLeast(10m).Should().Be(10m);
        15m.AtMost(10m).Should().Be(10m);
        1234567.89m.ToThousands().Should().Be("1,234,567.89");
    }
}
