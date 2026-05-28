using System;
using ATLAS.Kernel.Domain.Events;
using FluentAssertions;
using Xunit;

namespace ATLAS.Kernel.Tests.Domain.Events;

public class DomainEventTests
{
    private sealed record TestDomainEvent() : DomainEvent;

    [Fact]
    public void Base_properties_are_initialized()
    {
        var e = new TestDomainEvent();
        e.EventId.Should().NotBe(Guid.Empty);
        e.OccurredAt.Should().BeOnOrBefore(DateTimeOffset.UtcNow);
    }
}
