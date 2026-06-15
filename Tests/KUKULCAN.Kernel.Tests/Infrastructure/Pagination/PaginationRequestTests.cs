using FluentAssertions;
using KUKULCAN.Kernel.Infrastructure.Pagination;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Infrastructure.Pagination
{
    public class PaginationRequestTests
    {
        [Fact]
        public void Create_ClampsValues()
        {
            var p = PaginationRequest.Create(0, 500);
            p.Page.Should().Be(1);
            p.PageSize.Should().Be(200);
            p.Skip.Should().Be(0);
        }

        [Fact]
        public void Create_TrimsSearchAndPreservesSortOptions()
        {
            var p = PaginationRequest.Create(3, 25, "Name", SortOrder.Descending, "  term  ");

            p.Page.Should().Be(3);
            p.PageSize.Should().Be(25);
            p.SortBy.Should().Be("Name");
            p.SortOrder.Should().Be(SortOrder.Descending);
            p.Search.Should().Be("term");
            p.Skip.Should().Be(50);
        }

        [Fact]
        public void Create_ClampsPageSizeToMinimum()
        {
            var p = PaginationRequest.Create(1, 0);

            p.PageSize.Should().Be(1);
        }
    }
}
