using FluentAssertions;
using KUKULCAN.Kernel.Infrastructure.Extensions;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Infrastructure.Extensions;

public class StringExtensionsInfraTests
{
    [Fact]
    public void ToSlug_TruncatesAndNormalizes()
    {
        string s = "Hello, World!";
        s.ToSlug().Should().Be("hello-world");
    }

    [Fact]
    public void Mask_Works()
    {
        "4111111111111111".Mask().Should().Be("************1111");
        "abc".Mask(4).Should().Be("***");
        "".Mask().Should().Be("");
    }

    [Theory]
    [InlineData("Very long text", 8, "Very ...")]
    [InlineData("abc", 8, "abc")]
    [InlineData("abcdef", 2, "..")]
    public void Truncate_ReturnsExpectedValue(string input, int maxLength, string expected)
    {
        input.Truncate(maxLength).Should().Be(expected);
    }

    [Fact]
    public void CaseConversions_Work()
    {
        "hello world".ToTitleCase().Should().Be("Hello World");
        "CustomerStatus".ToSnakeCase().Should().Be("customer_status");
        "customer_status".ToCamelCase().Should().Be("customerStatus");
        "CustomerStatus".ToCamelCase().Should().Be("customerStatus");
    }

    [Fact]
    public void NullAndBase64Helpers_Work()
    {
        string? missing = null;

        missing.IsNullOrWhiteSpace().Should().BeTrue();
        "value".HasValue().Should().BeTrue();
        missing.HasValue().Should().BeFalse();
        missing.OrDefault("fallback").Should().Be("fallback");
        "value".OrDefault("fallback").Should().Be("value");
        "hello".ToBase64().FromBase64().Should().Be("hello");
    }
}
