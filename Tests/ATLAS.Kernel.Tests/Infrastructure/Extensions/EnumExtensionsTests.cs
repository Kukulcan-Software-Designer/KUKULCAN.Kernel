using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Infrastructure.Extensions;

namespace ATLAS.Kernel.Tests.Infrastructure.Extensions;

public class EnumExtensionsTests
{
    public enum E { A = 1, B = 2 }

    private enum E2 { [Display(Name = "Pretty")] Pretty = 1, [Description("Desc")] Other = 2 }

    [Fact]
    public void GetAll_ReturnsValues()
    {
        var all = EnumExtensions.GetAll<E>();
        all.Should().Contain([E.A, E.B]);
    }

    [Fact]
    public void ParseIgnoreCase_Works()
    {
        var val = EnumExtensions.ParseIgnoreCase<E>("b");
        val.Should().Be(E.B);
    }

    [Fact]
    public void GetDisplayName_And_GetDescription_Work()
    {
        var d1 = E2.Pretty.GetDisplayName();
        d1.Should().Be("Pretty");
        var d2 = E2.Other.GetDescription();
        d2.Should().Be("Desc");
        E.A.GetDisplayName().Should().Be("A");
        E.A.GetDescription().Should().Be("A");
    }

    [Fact]
    public void ParseIgnoreCase_Throws_WithValidValuesInMessage()
    {
        Action act = () => EnumExtensions.ParseIgnoreCase<E>("missing");

        act.Should().Throw<ArgumentException>().WithMessage("*A, B*");
    }

    [Theory]
    [InlineData("a", E.A)]
    [InlineData("B", E.B)]
    public void TryParseIgnoreCase_ReturnsValue_WhenValid(string value, E expected)
    {
        EnumExtensions.TryParseIgnoreCase<E>(value).Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("missing")]
    public void TryParseIgnoreCase_ReturnsNull_WhenBlankOrInvalid(string? value)
    {
        EnumExtensions.TryParseIgnoreCase<E>(value).Should().BeNull();
    }
}
