using ATLAS.Kernel.Abstractions.Interfaces.Domain;
using FluentAssertions;
using Xunit;

namespace ATLAS.Kernel.Tests.Domain;

public class DomainInterfacesTests
{
    [Fact]
    public void Interfaces_Exist()
    {
        typeof(IDomainEvent).IsInterface.Should().BeTrue();
        typeof(IIntegrationEvent).IsInterface.Should().BeTrue();
        typeof(IDomainEventHolder).IsInterface.Should().BeTrue();
    }
}
