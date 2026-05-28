using System;
using ATLAS.Kernel.Extensions;
using FluentAssertions;
using Xunit;

namespace ATLAS.Kernel.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void RemoveSpecialCharacters_StripsNewlinesAndPunctuation()
    {
        string s = "Hello\nWorld,&\"";
        s.RemoveSpecialCharacters().Should().Be("HelloWorld");
    }

    [Fact]
    public void ToDateFromStringIso8601Format_ParsesValid()
    {
        DateTime? dt = "20260301".ToDateFromStringIso8601Format();
        dt.Should().NotBeNull();
        dt.Value.Year.Should().Be(2026);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("2026-03-01")]
    public void ToDateFromStringIso8601Format_ReturnsNull_WhenInputIsBlankOrInvalid(string? input)
    {
        input.ToDateFromStringIso8601Format().Should().BeNull();
    }

    [Fact]
    public void ToDateFromStringWithHhmm_ParsesValid()
    {
        DateTime? dt = "202603011030".ToDateFromStringWithHhmm();
        dt.Should().NotBeNull();
        dt.Value.Hour.Should().Be(10);
        dt.Value.Minute.Should().Be(30);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("2026030110")]
    public void ToDateFromStringWithHhmm_ReturnsNull_WhenInputIsBlankOrInvalid(string? input)
    {
        input.ToDateFromStringWithHhmm().Should().BeNull();
    }

    [Fact]
    public void RemoveSpecialCharacters_ReturnsEmpty_ForBlankInput()
    {
        string? value = null;

        value.RemoveSpecialCharacters().Should().BeEmpty();
        "   ".RemoveSpecialCharacters().Should().BeEmpty();
    }
}
