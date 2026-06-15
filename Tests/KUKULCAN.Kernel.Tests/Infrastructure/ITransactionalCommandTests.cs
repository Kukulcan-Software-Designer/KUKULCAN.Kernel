using FluentAssertions;
using KUKULCAN.Kernel.Abstractions.Interfaces.Infrastructure;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Infrastructure
{
    public class ITransactionalCommandTests
    {
        private sealed record FakeCmd : ITransactionalCommand;

        [Fact]
        public void MarkerInterface_IsAssignable()
        {
            typeof(ITransactionalCommand).IsAssignableFrom(typeof(FakeCmd)).Should().BeTrue();
        }
    }
}
