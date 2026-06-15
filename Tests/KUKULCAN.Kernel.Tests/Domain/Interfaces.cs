using FluentAssertions;
using KUKULCAN.Kernel.Abstractions.Interfaces.Domain;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain;

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
