using System.Collections.Generic;
using System.Linq;
using System;
using ATLAS.Kernel.Infrastructure.Extensions;
using ATLAS.Kernel.Infrastructure.Pagination;
using ATLAS.Kernel.Domain.Specifications;
using FluentAssertions;
using Xunit;
using System.Linq.Expressions;

namespace ATLAS.Kernel.Tests.Infrastructure.Extensions;

public class QueryableExtensionsTests
{
    private class E { public int Id { get; init; } }
    private sealed class GreaterThanSpec(int threshold) : Specification<E>
    {
        public override Expression<Func<E, bool>> ToExpression() => e => e.Id > threshold;
    }

    [Fact]
    public void ApplyPaging_SkipsAndTakes()
    {
        IQueryable<E> q = Enumerable.Range(1,10).Select(i => new E { Id = i }).AsQueryable();
        List<E> page = q.ApplyPaging(new PaginationRequest { Page = 2, PageSize = 3 }).ToList();
        page.Select(x => x.Id).Should().Equal(4, 5, 6);
    }

    [Fact]
    public void ApplyOrdering_ById_Works()
    {
        IQueryable<E> q = new[] { new E { Id = 2 }, new E { Id = 1 } }.AsQueryable();
        List<E> ordered = q.ApplyOrdering("Id").ToList();
        ordered.Select(x => x.Id).Should().Equal(1, 2);
    }

    [Fact]
    public void ApplyOrdering_DescendingAndInvalidProperty_Work()
    {
        IQueryable<E> q = new[] { new E { Id = 1 }, new E { Id = 2 } }.AsQueryable();

        q.ApplyOrdering("id", SortOrder.Descending).Select(x => x.Id).Should().Equal(2, 1);
        q.ApplyOrdering("missing").Select(x => x.Id).Should().Equal(1, 2);
        q.ApplyOrdering(null).Select(x => x.Id).Should().Equal(1, 2);
    }

    [Fact]
    public void ApplySpecification_FiltersQuery()
    {
        IQueryable<E> q = Enumerable.Range(1, 5).Select(i => new E { Id = i }).AsQueryable();

        List<E> filtered = q.ApplySpecification(new GreaterThanSpec(3)).ToList();

        filtered.Select(x => x.Id).Should().Equal(4, 5);
    }

    [Fact]
    public void ApplySpecification_AndApplyPaging_ThrowOnNullArguments()
    {
        IQueryable<E> q = Enumerable.Range(1, 5).Select(i => new E { Id = i }).AsQueryable();

        Action nullSpec = () => q.ApplySpecification(null!).ToList();
        Action nullPaging = () => q.ApplyPaging(null!).ToList();

        nullSpec.Should().Throw<ArgumentNullException>();
        nullPaging.Should().Throw<ArgumentNullException>();
    }
}
