using System;
using FluentAssertions;
using Xunit;
using ATLAS.Kernel.Domain.Specifications;
using System.Linq.Expressions;

namespace ATLAS.Kernel.Tests.Domain.Specifications
{
    public class SpecificationTests
    {
        private sealed class GreaterThanSpec(int threshold) : Specification<int>
        {
            public override Expression<Func<int, bool>> ToExpression() => x => x > threshold;
        }

        private sealed class EvenSpec : Specification<int>
        {
            public override Expression<Func<int, bool>> ToExpression() => x => x % 2 == 0;
        }

        [Fact]
        public void And_ComposesToRequireBoth()
        {
            var spec = new GreaterThanSpec(5).And(new EvenSpec());
            spec.IsSatisfiedBy(8).Should().BeTrue(); // >5 and even
            spec.IsSatisfiedBy(7).Should().BeFalse(); // >5 but odd
            spec.IsSatisfiedBy(4).Should().BeFalse(); // even but not >5
        }

        [Fact]
        public void Or_ComposesToEither()
        {
            var spec = new GreaterThanSpec(5).Or(new EvenSpec());
            spec.IsSatisfiedBy(8).Should().BeTrue();
            spec.IsSatisfiedBy(7).Should().BeTrue();
            spec.IsSatisfiedBy(4).Should().BeTrue();
            spec.IsSatisfiedBy(3).Should().BeFalse();
        }

        [Fact]
        public void Not_InvertsResult()
        {
            var spec = new GreaterThanSpec(5).Not();
            spec.IsSatisfiedBy(8).Should().BeFalse();
            spec.IsSatisfiedBy(3).Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_Throws_WhenEntityIsNull()
        {
            var spec = new ExpressionSpec<object>(_ => true);

            Action act = () => spec.IsSatisfiedBy(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AndOr_Throw_WhenOtherSpecificationIsNull()
        {
            var spec = new GreaterThanSpec(5);

            Action and = () => spec.And(null!);
            Action or = () => spec.Or(null!);

            and.Should().Throw<ArgumentNullException>();
            or.Should().Throw<ArgumentNullException>();
        }

        private sealed class ExpressionSpec<T>(Expression<Func<T, bool>> expression) : Specification<T>
        {
            public override Expression<Func<T, bool>> ToExpression() => expression;
        }
    }
}
