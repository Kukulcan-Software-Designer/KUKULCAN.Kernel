using System;
using System.Collections.Generic;
using FluentAssertions;
using KUKULCAN.Kernel.Infrastructure.Pagination;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Infrastructure.Pagination;

public class PagedResultTests
{
    [Fact]
    public void Create_ComputesTotalsCorrectly()
    {
        var items = new List<int> { 1, 2 } as IReadOnlyList<int>;
        var req = new PaginationRequest { Page = 1, PageSize = 10 };
        PagedResult<int> page = PagedResult<int>.Create(items, 25, req);
        page.TotalPages.Should().Be(3);
        page.HasNextPage.Should().BeTrue();
        page.FirstItemIndex.Should().Be(1);
        page.LastItemIndex.Should().Be(2);
    }

    [Fact]
    public void Create_ComputesPreviousPageAndIndexes_ForLaterPage()
    {
        var items = new List<int> { 11, 12, 13 } as IReadOnlyList<int>;
        var req = new PaginationRequest { Page = 2, PageSize = 10 };

        PagedResult<int> page = PagedResult<int>.Create(items, 25, req);

        page.HasPreviousPage.Should().BeTrue();
        page.HasNextPage.Should().BeTrue();
        page.FirstItemIndex.Should().Be(11);
        page.LastItemIndex.Should().Be(13);
    }

    [Fact]
    public void Empty_ReturnsZeroMetadata()
    {
        PagedResult<int> page = PagedResult<int>.Empty(new PaginationRequest { Page = 3, PageSize = 10 });

        page.Items.Should().BeEmpty();
        page.TotalCount.Should().Be(0);
        page.TotalPages.Should().Be(0);
        page.FirstItemIndex.Should().Be(0);
        page.LastItemIndex.Should().Be(0);
        page.HasNextPage.Should().BeFalse();
        page.HasPreviousPage.Should().BeTrue();
    }

    [Fact]
    public void Map_ProjectsItemsAndPreservesMetadata()
    {
        var page = PagedResult<int>.Create([1, 2], 10, new PaginationRequest { Page = 2, PageSize = 2 });

        PagedResult<string> mapped = page.Map(x => $"#{x}");

        mapped.Items.Should().Equal("#1", "#2");
        mapped.TotalCount.Should().Be(10);
        mapped.Page.Should().Be(2);
        mapped.PageSize.Should().Be(2);
    }

    [Fact]
    public void CreateEmptyAndMap_ThrowOnNullArguments()
    {
        var pagination = new PaginationRequest();
        var page = PagedResult<int>.Create([1], 1, pagination);

        Action nullItems = () => PagedResult<int>.Create(null!, 0, pagination);
        Action nullPaginationCreate = () => PagedResult<int>.Create([1], 1, null!);
        Action nullPaginationEmpty = () => PagedResult<int>.Empty(null!);
        Action nullMapper = () => page.Map<string>(null!);

        nullItems.Should().Throw<ArgumentNullException>();
        nullPaginationCreate.Should().Throw<ArgumentNullException>();
        nullPaginationEmpty.Should().Throw<ArgumentNullException>();
        nullMapper.Should().Throw<ArgumentNullException>();
    }
}
