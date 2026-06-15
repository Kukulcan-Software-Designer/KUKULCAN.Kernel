using FluentAssertions;
using KUKULCAN.Kernel.Extensions;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Extensions;

public class IntegerExtensionsTests
{
    [Fact]
    public void ParseOrDefault_ReturnsParsedValue()
    {
        IntegerExtensions.ParseOrDefault("123", null).Should().Be(123);
    }

    [Fact]
    public void ParseOrDefault_ReturnsDefaultOnInvalid()
    {
        IntegerExtensions.ParseOrDefault("notint", 42).Should().Be(42);
    }

    [Fact]
    public void ParseOrDefault_ReturnsNull_WhenInvalidAndNoDefault()
    {
        IntegerExtensions.ParseOrDefault("notint", null).Should().BeNull();
    }

    [Fact]
    public void ParseOrDefault_ReturnsDefaultOnOverflow()
    {
        IntegerExtensions.ParseOrDefault("999999999999999999999999999999", -1).Should().Be(-1);
    }
}
