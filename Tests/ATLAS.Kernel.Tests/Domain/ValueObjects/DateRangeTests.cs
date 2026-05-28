using System;
using ATLAS.Kernel.Domain.Result;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Domain.ValueObjects;

namespace ATLAS.Kernel.Tests.Domain.ValueObjects;

public class DateRangeTests
{
    [Fact]
    public void Create_Bounded_ContainsAndDuration()
    {
        var from = new DateOnly(2026,1,1);
        var to = new DateOnly(2026,12,31);
        Result<DateRange> r = DateRange.Create(from, to);
        r.IsSuccess.Should().BeTrue();
        DateRange dr = r.Value;
        dr.Contains(new DateOnly(2026,6,1)).Should().BeTrue();
        dr.Contains(new DateOnly(2025,12,31)).Should().BeFalse();
        dr.Duration.Should().Be(TimeSpan.FromDays(364));
        dr.ToString().Should().Be("2026-01-01..2026-12-31");
        dr.IsOpenEnded.Should().BeFalse();
    }

    [Fact]
    public void CreateOpenEnded_HasMaxDuration()
    {
        var dr = DateRange.CreateOpenEnded(new DateOnly(2026,1,1));
        dr.IsOpenEnded.Should().BeTrue();
        dr.Duration.Should().Be(TimeSpan.MaxValue);
        dr.Contains(DateOnly.MaxValue).Should().BeTrue();
        dr.ToString().Should().Be("2026-01-01..\u221e");
    }

    [Fact]
    public void Create_Fails_WhenStartIsAfterEnd()
    {
        Result<DateRange> result = DateRange.Create(new DateOnly(2026, 2, 1), new DateOnly(2026, 1, 1));

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DateRange.Invalid");
    }

    [Fact]
    public void Overlaps_ReturnsExpectedResultsForBoundedAndOpenRanges()
    {
        DateRange first = DateRange.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 31)).Value;
        DateRange overlapping = DateRange.Create(new DateOnly(2026, 1, 31), new DateOnly(2026, 2, 15)).Value;
        DateRange separate = DateRange.Create(new DateOnly(2026, 2, 1), new DateOnly(2026, 2, 15)).Value;
        DateRange openEnded = DateRange.CreateOpenEnded(new DateOnly(2026, 1, 15));

        first.Overlaps(overlapping).Should().BeTrue();
        first.Overlaps(separate).Should().BeFalse();
        first.Overlaps(openEnded).Should().BeTrue();
    }
}
