using System;
using FluentAssertions;
using KUKULCAN.Kernel.Domain.Result;
using KUKULCAN.Kernel.Domain.ValueObjects;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Create_ValidMoney_Succeeds()
    {
        Result<Money> r = Money.Create(100m, "EUR");
        r.IsSuccess.Should().BeTrue();
        r.Value.Amount.Should().Be(100m);
        r.Value.CurrencyCode.Should().Be("EUR");
        r.Value.ToString().Should().Be("100.00 EUR");
    }

    [Fact]
    public void Create_NegativeAmount_Fails()
    {
        Result<Money> r = Money.Create(-1m, "EUR");
        r.IsFailure.Should().BeTrue();
        r.Error.Code.Should().Be("Money.Amount.Negative");
    }

    [Theory]
    [InlineData("")]
    [InlineData("EU")]
    [InlineData("EURO")]
    public void Create_InvalidCurrency_Fails(string currency)
    {
        Result<Money> r = Money.Create(1m, currency);

        r.IsFailure.Should().BeTrue();
        r.Error.Code.Should().Be("Money.Currency.Invalid");
    }

    [Fact]
    public void Zero_CreatesZeroMoneyInCurrency()
    {
        Money.Zero("eur").Should().Be(Money.Create(0m, "EUR").Value);
    }

    [Fact]
    public void AddAndSubtract_Operators_Work()
    {
        Money a = Money.Create(50m, "EUR").Value;
        Money b = Money.Create(20m, "EUR").Value;
        (a + b).Amount.Should().Be(70m);
        (a - b).Amount.Should().Be(30m);
        (b * 2m).Amount.Should().Be(40m);
        a.Round(0).Amount.Should().Be(50m);
    }

    [Fact]
    public void Percentage_ComputesFraction()
    {
        Money m = Money.Create(100m, "EUR").Value;
        Money p = m.Percentage(10m);
        p.Amount.Should().BeApproximately(10m, 0.000001m);
    }

    [Fact]
    public void ArithmeticAndComparison_Throw_WhenCurrenciesDiffer()
    {
        Money eur = Money.Create(10m, "EUR").Value;
        Money usd = Money.Create(5m, "USD").Value;

        Action add = () => _ = eur + usd;
        Action subtract = () => _ = eur - usd;
        Action compare = () => _ = eur > usd;

        add.Should().Throw<InvalidOperationException>().WithMessage("*different currencies*");
        subtract.Should().Throw<InvalidOperationException>().WithMessage("*different currencies*");
        compare.Should().Throw<InvalidOperationException>().WithMessage("*different currencies*");
    }

    [Fact]
    public void Subtract_Throws_WhenResultWouldBeNegative()
    {
        Money low = Money.Create(5m, "EUR").Value;
        Money high = Money.Create(10m, "EUR").Value;

        Action act = () => _ = low - high;

        act.Should().Throw<InvalidOperationException>().WithMessage("*negative*");
    }

    [Fact]
    public void ComparisonOperators_WorkForSameCurrency()
    {
        Money low = Money.Create(5m, "EUR").Value;
        Money high = Money.Create(10m, "EUR").Value;
        Money same = Money.Create(10m, "EUR").Value;

        (high > low).Should().BeTrue();
        (low < high).Should().BeTrue();
        (high >= same).Should().BeTrue();
        (low <= same).Should().BeTrue();
    }
}
