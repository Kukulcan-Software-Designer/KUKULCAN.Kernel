using System;
using FluentAssertions;
using KUKULCAN.Kernel.Infrastructure.Extensions;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Infrastructure.Extensions;

public class GuidExtensionsTests
{
    [Fact]
    public void IsEmpty_HasValue_ToShortString_NewSequential()
    {
        Guid.Empty.IsEmpty().Should().BeTrue();
        Guid.Empty.HasValue().Should().BeFalse();
        Guid.NewGuid().HasValue().Should().BeTrue();
        var guid = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef");
        guid.ToShortString().Should().Be("01234567");
        GuidExtensions.NewSequential().Should().NotBe(Guid.Empty);
        GuidExtensions.NewSequentialAtEnd().Should().NotBe(Guid.Empty);
    }
}
