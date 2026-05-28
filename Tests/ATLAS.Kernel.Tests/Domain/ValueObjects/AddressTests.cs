using ATLAS.Kernel.Domain.Result;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Domain.ValueObjects;

namespace ATLAS.Kernel.Tests.Domain.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Create_ValidAddress_Succeeds()
    {
        Result<Address> r = Address.Create(" Calle Mayor 1 ", " Madrid ", " 28001 ", " es ", " 2B ", " Madrid ");
        r.IsSuccess.Should().BeTrue();
        r.Value.City.Should().Be("Madrid");
        r.Value.CountryCode.Should().Be("ES");
        r.Value.Street.Should().Be("Calle Mayor 1");
        r.Value.Street2.Should().Be("2B");
        r.Value.State.Should().Be("Madrid");
        r.Value.PostalCode.Should().Be("28001");
        r.Value.ToString().Should().Be("Calle Mayor 1, 2B, Madrid, Madrid, 28001, ES");
    }

    [Fact]
    public void Create_InvalidCountry_Fails()
    {
        Result<Address> r = Address.Create("S", "City", "123", "XYZ");
        r.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "City", "12345", "ES", "Address.Street.Empty")]
    [InlineData("Street", "", "12345", "ES", "Address.City.Empty")]
    [InlineData("Street", "City", "", "ES", "Address.PostalCode.Empty")]
    public void Create_ReturnsValidationFailure_WhenRequiredFieldIsMissing(
        string street,
        string city,
        string postalCode,
        string countryCode,
        string expectedCode)
    {
        Result<Address> result = Address.Create(street, city, postalCode, countryCode);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(expectedCode);
    }

    [Fact]
    public void Equality_IsCaseInsensitiveForAddressComponents()
    {
        Address.Create("Street", "Madrid", "28001", "ES").Value
            .Should()
            .Be(Address.Create("street", "madrid", "28001", "es").Value);
    }
}
