using System;
using ATLAS.Kernel.Domain.Events;
using FluentAssertions;
using Xunit;

namespace ATLAS.Kernel.Tests.Domain.Events;

public class IntegrationEventTests
{
    private sealed record TestIntegrationEvent : IntegrationEvent;

    [Fact]
    public void Base_properties_and_type_are_correct()
    {
        var e = new TestIntegrationEvent();
        e.EventId.Should().NotBe(Guid.Empty);
        e.OccurredAt.Should().BeOnOrBefore(DateTimeOffset.UtcNow);
        e.EventType.Should().Contain(nameof(TestIntegrationEvent));
    }
}
