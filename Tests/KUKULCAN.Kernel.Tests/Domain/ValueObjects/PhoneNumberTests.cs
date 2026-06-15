using FluentAssertions;
using KUKULCAN.Kernel.Domain.Result;
using KUKULCAN.Kernel.Domain.ValueObjects;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.ValueObjects;

public class PhoneNumberTests
{
    [Fact]
    public void Create_ValidE164_Succeeds()
    {
        Result<PhoneNumber> r = PhoneNumber.Create(" +34 612-345-678 ");
        r.IsSuccess.Should().BeTrue();
        r.Value.Value.Should().Be("+34612345678");
        r.Value.CountryCode.Should().Be("+34");
        r.Value.ToString().Should().Be("+34612345678");
    }

    [Theory]
    [InlineData(null, "PhoneNumber.Empty")]
    [InlineData("", "PhoneNumber.Empty")]
    [InlineData("12345", "PhoneNumber.Format.Invalid")]
    [InlineData("+0123456789", "PhoneNumber.Format.Invalid")]
    public void Create_Invalid_Fails(string? number, string expectedCode)
    {
        Result<PhoneNumber> r = PhoneNumber.Create(number);
        r.IsFailure.Should().BeTrue();
        r.Error.Code.Should().Be(expectedCode);
    }
}
