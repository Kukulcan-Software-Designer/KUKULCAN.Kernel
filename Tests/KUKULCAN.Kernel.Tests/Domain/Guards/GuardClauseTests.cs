using System;
using System.Collections.Generic;
using FluentAssertions;
using KUKULCAN.Kernel.Domain.Guards;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.Guards;

public class GuardClauseTests
{
    [Fact]
    public void Null_ReturnsValue_AndThrowsForNull()
    {
        var value = new object();

        Guard.Against.Null(value, "value").Should().BeSameAs(value);
        Action act = () => Guard.Against.Null<object>(null, "value");
        act.Should().Throw<ArgumentNullException>().WithParameterName("value");
    }

    [Fact]
    public void NullOrEmpty_And_NullOrWhiteSpace_ValidateStrings()
    {
        Guard.Against.NullOrEmpty("value", "p").Should().Be("value");
        Guard.Against.NullOrWhiteSpace(" value ", "p").Should().Be(" value ");

        Action empty = () => _ = Guard.Against.NullOrEmpty("", "p");
        Action whitespace = () => _ = Guard.Against.NullOrWhiteSpace("   ", "p");

        empty.Should().Throw<ArgumentException>().WithParameterName("p");
        whitespace.Should().Throw<ArgumentException>().WithParameterName("p");
    }

    [Fact]
    public void NullOrEmpty_ValidatesCollectionsWithoutConsumingThem()
    {
        IEnumerable<int> values = YieldValues();

        Guard.Against.NullOrEmpty(values, "values").Should().Equal(1, 2);

        Action nullCollection = () => _ = Guard.Against.NullOrEmpty<int>(null, "values");
        Action emptyCollection = () => _ = Guard.Against.NullOrEmpty(Array.Empty<int>(), "values");

        nullCollection.Should().Throw<ArgumentException>().WithParameterName("values");
        emptyCollection.Should().Throw<ArgumentException>().WithParameterName("values");
    }

    [Fact]
    public void Default_Throws_ForDefaultValue()
    {
        Guard.Against.Default(Guid.NewGuid(), "id").Should().NotBe(Guid.Empty);

        Action act = () => _ = Guard.Against.Default(Guid.Empty, "id");

        act.Should().Throw<ArgumentException>().WithParameterName("id");
    }

    [Fact]
    public void NumericGuards_ReturnValidValues_AndThrowForInvalidValues()
    {
        Guard.Against.Negative(0, "n").Should().Be(0);
        Guard.Against.Negative(1m, "n").Should().Be(1m);
        Guard.Against.NegativeOrZero(1, "n").Should().Be(1);
        Guard.Against.NegativeOrZero(1m, "n").Should().Be(1m);
        Guard.Against.Positive(1m, "n").Should().Be(1m);

        Action negativeInt = () => _ = Guard.Against.Negative(-1, "n");
        Action negativeDecimal = () => _ = Guard.Against.Negative(-1m, "n");
        Action zeroInt = () => _ = Guard.Against.NegativeOrZero(0, "n");
        Action zeroDecimal = () => _ = Guard.Against.NegativeOrZero(0m, "n");
        Action positiveAlias = () => _ = Guard.Against.Positive(0m, "n");

        negativeInt.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        negativeDecimal.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        zeroInt.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        zeroDecimal.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        positiveAlias.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
    }

    [Fact]
    public void OutOfRange_ReturnsValueInsideInclusiveRange_AndThrowsOutside()
    {
        Guard.Against.OutOfRange(5, 1, 10, "n").Should().Be(5);

        Action below = () => _ = Guard.Against.OutOfRange(0, 1, 10, "n");
        Action above = () => _ = Guard.Against.OutOfRange(11, 1, 10, "n");

        below.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        above.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
    }

    [Fact]
    public void LengthGuards_ReturnValueWhenValid_AndThrowWhenInvalid()
    {
        Guard.Against.MaxLength("abc", 3, "s").Should().Be("abc");
        Guard.Against.MinLength("abc", 3, "s").Should().Be("abc");

        Action tooLong = () => _ = Guard.Against.MaxLength("abcd", 3, "s");
        Action tooShort = () => _ = Guard.Against.MinLength("ab", 3, "s");

        tooLong.Should().Throw<ArgumentException>().WithParameterName("s");
        tooShort.Should().Throw<ArgumentException>().WithParameterName("s");
    }

    private static IEnumerable<int> YieldValues()
    {
        yield return 1;
        yield return 2;
    }
}
