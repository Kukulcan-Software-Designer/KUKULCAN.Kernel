using FluentAssertions;
using KUKULCAN.Kernel.Domain.ValueObjects;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.ValueObjects;

public class LanguageCodeTests
{
    [Fact]
    public void Create_Valid_ReturnsValueAndFallbackChain()
    {
        var r = LanguageCode.Create("es-ES");

        r.IsSuccess.Should().BeTrue();
        r.Value.Value.Should().Be("es-ES");
        r.Value.Language.Should().Be("es");
        r.Value.Region.Should().Be("ES");
        r.Value.ToString().Should().Be("es-ES");
        string value = r.Value;
        value.Should().Be("es-ES");
        r.Value.FallbackChain.Should().ContainInOrder(new[] { "es-ES", "es", "en" });
    }

    [Fact]
    public void Create_LanguageOnly_ReturnsLanguageAndUltimateFallback()
    {
        var r = LanguageCode.Create("EN");

        r.IsSuccess.Should().BeTrue();
        r.Value.Value.Should().Be("en");
        r.Value.Language.Should().Be("en");
        r.Value.Region.Should().BeNull();
        r.Value.FallbackChain.Should().Equal("en", "en");
    }

    [Theory]
    [InlineData(null, "LanguageCode.Empty")]
    [InlineData("", "LanguageCode.Empty")]
    [InlineData("not-a-lang", "LanguageCode.Format.Invalid")]
    public void Create_Invalid_ReturnsFailure(string? tag, string expectedCode)
    {
        var result = LanguageCode.Create(tag);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(expectedCode);
    }
}
