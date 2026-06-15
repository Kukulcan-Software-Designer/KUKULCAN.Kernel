using System;
using FluentAssertions;
using KUKULCAN.Kernel.Infrastructure.Extensions;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Infrastructure.Extensions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void StartAndEndHelpers_ReturnExpectedBoundaries()
    {
        var dt = new DateTimeOffset(2026, 3, 15, 10, 30, 45, TimeSpan.FromHours(1));

        dt.StartOfDay().Should().Be(new DateTimeOffset(2026, 3, 15, 0, 0, 0, TimeSpan.FromHours(1)));
        dt.EndOfDay().Should().Be(new DateTimeOffset(2026, 3, 15, 23, 59, 59, 999, TimeSpan.FromHours(1)).AddTicks(9999));
        dt.StartOfMonth().Should().Be(new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.FromHours(1)));
        dt.EndOfMonth().Should().Be(new DateTimeOffset(2026, 3, 31, 23, 59, 59, 999, TimeSpan.FromHours(1)).AddTicks(9999));
    }

    [Fact]
    public void ToUnixSeconds_And_FromUnixSeconds_Roundtrip()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        long secs = now.ToUnixSeconds();
        DateTimeOffset round = DateTimeExtensions.FromUnixSeconds(secs);
        round.ToUnixSeconds().Should().Be(secs);
        now.ToUnixMilliseconds().Should().Be(now.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void IsWeekend_IsWeekday_Work()
    {
        var saturday = new DateTimeOffset(2026,3,7,0,0,0, TimeSpan.Zero); // Saturday

        saturday.IsWeekend().Should().BeTrue();
        saturday.IsWeekday().Should().BeFalse();
    }

    [Fact]
    public void ToIso8601_FormatsUtc()
    {
        var dt = new DateTimeOffset(2026,3,1,11,30,0, TimeSpan.FromHours(1));

        dt.ToIso8601().Should().Be("2026-03-01T10:30:00Z");
    }

    [Theory]
    [InlineData(-2, "2m ago")]
    [InlineData(3, "3m from now")]
    public void ToRelativeString_FormatsPastAndFutureMinutes(int minutes, string expected)
    {
        var now = new DateTimeOffset(2026, 3, 1, 10, 0, 0, TimeSpan.Zero);

        now.AddMinutes(minutes).ToRelativeString(now).Should().Be(expected);
        now.AddSeconds(-1).ToRelativeString(now).Should().Be("just now");
    }
}
