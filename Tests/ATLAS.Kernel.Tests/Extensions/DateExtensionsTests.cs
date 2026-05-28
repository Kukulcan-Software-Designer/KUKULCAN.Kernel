using System;
using System.Globalization;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Extensions;

namespace ATLAS.Kernel.Tests.Extensions;

public class DateExtensionsTests
{
    [Fact]
    public void ToDateFormat_Null_ReturnsEmptyString()
    {
        DateTime? d = null;
        d.ToDateFormat().Should().BeEmpty();
        d.ToDateFormatWithTime().Should().BeEmpty();
    }

    [Fact]
    public void ToDateFormatWithTime_Date_ReturnsFormatted()
    {
        var dt = new DateTime(2026, 3, 1, 10, 30, 0, DateTimeKind.Utc);
        dt.ToDateFormatWithTime().Should().Be("20260301103000");
        dt.ToDateFormatWithTimeIso8601().Should().StartWith("2026-03-01T10:30:00");
    }

    [Fact]
    public void ToDateFormat_NonNull_ReturnsDateOnlyFormat()
    {
        DateTime? dt = new DateTime(2026, 3, 1, 10, 30, 0);

        dt.ToDateFormat().Should().Be("20260301");
    }

    [Fact]
    public void ParsePart_ReturnsMatchedDate_WhenInputContainsFormattedPart()
    {
        DateTime? result = DateExtensions.ParsePart(
            "invoice-20260301.csv",
            "\\d{8}",
            "yyyyMMdd",
            CultureInfo.InvariantCulture,
            defaultValue: null);

        result.Should().Be(new DateTime(2026, 3, 1));
    }

    [Fact]
    public void ParsePart_ReturnsDefault_WhenNoMatchOrInvalidRegex()
    {
        var fallback = new DateTime(2000, 1, 1);

        DateExtensions.ParsePart("invoice.csv", "\\d{8}", "yyyyMMdd", CultureInfo.InvariantCulture, fallback)
            .Should().Be(fallback);
        DateExtensions.ParsePart("invoice.csv", "[", "yyyyMMdd", CultureInfo.InvariantCulture, fallback)
            .Should().Be(fallback);
    }

    [Fact]
    public void ParseExactOrDefault_ReturnsDefault_WhenFormatIsInvalid()
    {
        var fallback = new DateTime(2000, 1, 1);

        DateExtensions.ParseExactOrDefault("not-a-date", "yyyyMMdd", CultureInfo.InvariantCulture, fallback)
            .Should().Be(fallback);
        DateExtensions.ParseExactOrDefault("20260301", "bad-format", CultureInfo.InvariantCulture, fallback)
            .Should().Be(fallback);
    }
}
