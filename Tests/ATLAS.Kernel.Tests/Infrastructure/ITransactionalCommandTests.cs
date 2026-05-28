using FluentAssertions;
using Xunit;
using ATLAS.Kernel.Abstractions.Interfaces.Infrastructure;

namespace ATLAS.Kernel.Tests.Infrastructure
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
