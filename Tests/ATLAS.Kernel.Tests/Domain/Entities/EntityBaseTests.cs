using System;
using FluentAssertions;
using Xunit;
using ATLAS.Kernel.Domain.Entities;

namespace ATLAS.Kernel.Tests.Domain.Entities
{
    public class EntityBaseTests
    {
        private sealed class E(Guid id) : EntityBase<Guid>(id);
        private sealed class OtherE(Guid id) : EntityBase<Guid>(id);

        [Fact]
        public void Constructor_Throws_OnDefaultId()
        {
            Action act = () => _ = new E(Guid.Empty);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Entities_WithSameId_AreEqual()
        {
            var id = Guid.NewGuid();
            var a = new E(id);
            var b = new E(id);
            (a == b).Should().BeTrue();
            a.Equals(b).Should().BeTrue();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Fact]
        public void Entities_WithDifferentIdOrType_AreNotEqual()
        {
            var id = Guid.NewGuid();
            var a = new E(id);

            a.Equals(new E(Guid.NewGuid())).Should().BeFalse();
            a.Equals(new OtherE(id)).Should().BeFalse();
            a.Equals(null).Should().BeFalse();
            (a != new E(Guid.NewGuid())).Should().BeTrue();
            (a == null).Should().BeFalse();
        }
    }
}
