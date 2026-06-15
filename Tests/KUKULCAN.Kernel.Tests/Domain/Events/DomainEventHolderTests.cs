using System;
using FluentAssertions;
using KUKULCAN.Kernel.Domain.Entities;
using KUKULCAN.Kernel.Domain.Events;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.Events;

public class DomainEventHolderTests
{
    private sealed record TestDomainEvent(Guid Id) : DomainEvent;
    private sealed class Agg(Guid id) : AggregateRoot<Guid>(id);

    [Fact]
    public void AddAndClearDomainEvents_Work()
    {
        var a = new Agg(Guid.NewGuid());

        a.AddDomainEvent(new TestDomainEvent(Guid.NewGuid()));

        a.DomainEvents.Should().NotBeEmpty();
        a.ClearDomainEvents();
        a.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void AddDomainEvent_Throws_WhenEventIsNull()
    {
        var a = new Agg(Guid.NewGuid());

        Action act = () => a.AddDomainEvent(null!);

        act.Should().Throw<ArgumentNullException>();
    }
}
