using System.Collections.Generic;
using System.Linq;
using System;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Infrastructure.Extensions;
using ATLAS.Kernel.Infrastructure.Pagination;

namespace ATLAS.Kernel.Tests.Infrastructure.Extensions;

public class CollectionExtensionsTests
{
    [Fact]
    public void IsNullOrEmpty_And_HasItems_WorkForNullEmptyAndNonEmpty()
    {
        IEnumerable<int>? missing = null;
        IEnumerable<int> empty = [];
        IEnumerable<int> values = [1];

        missing.IsNullOrEmpty().Should().BeTrue();
        empty.IsNullOrEmpty().Should().BeTrue();
        values.IsNullOrEmpty().Should().BeFalse();
        missing.HasItems().Should().BeFalse();
        empty.HasItems().Should().BeFalse();
        values.HasItems().Should().BeTrue();
    }

    [Fact]
    public void Batch_SplitsSequence()
    {
        IEnumerable<int> seq = Enumerable.Range(1,5);
        List<IReadOnlyList<int>> batches = seq.Batch(2).ToList();
        batches.Should().HaveCount(3);
        batches[0].Should().Equal(new[] {1,2});
        batches[2].Should().Equal(new[] {5});
    }

    [Fact]
    public void Batch_Throws_WhenSourceIsNullOrBatchSizeIsInvalid()
    {
        IEnumerable<int>? source = null;

        Action nullSource = () => source!.Batch(2).ToList();
        Action invalidSize = () => new[] { 1 }.Batch(0).ToList();

        nullSource.Should().Throw<ArgumentNullException>();
        invalidSize.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ForEach_ExecutesActionForEveryItem()
    {
        var sum = 0;

        new[] { 1, 2, 3 }.ForEach(x => sum += x);

        sum.Should().Be(6);
    }

    [Fact]
    public void ForEach_Throws_WhenSourceOrActionIsNull()
    {
        IEnumerable<int>? source = null;
        Action<int>? action = null;

        Action nullSource = () => source!.ForEach(_ => { });
        Action nullAction = () => new[] { 1 }.ForEach(action!);

        nullSource.Should().Throw<ArgumentNullException>();
        nullAction.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToPagedResult_ReturnsPage()
    {
        IEnumerable<int> seq = Enumerable.Range(1,7);
        PagedResult<int> page = seq.ToPagedResult(new PaginationRequest { Page = 1, PageSize = 3 });
        page.Items.Should().Equal(new[] {1,2,3});
        page.TotalCount.Should().Be(7);
    }

    [Fact]
    public void ToPagedResult_Throws_WhenArgumentsAreNull()
    {
        IEnumerable<int>? source = null;
        var pagination = new PaginationRequest();

        Action nullSource = () => source!.ToPagedResult(pagination);
        Action nullPagination = () => new[] { 1 }.ToPagedResult(null!);

        nullSource.Should().Throw<ArgumentNullException>();
        nullPagination.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AsReadOnlyList_ReusesReadOnlyLists_AndMaterializesOtherSequences()
    {
        IReadOnlyList<int> readOnly = new List<int> { 1, 2 };
        IEnumerable<int> sequence = Enumerable.Range(1, 2);

        readOnly.AsReadOnlyList().Should().BeSameAs(readOnly);
        sequence.AsReadOnlyList().Should().Equal(1, 2);
    }

    [Fact]
    public void NullToEmpty_ReturnsEmptyForNull_AndOriginalItemsForNonNull()
    {
        IEnumerable<int>? missing = null;

        missing.NullToEmpty().Should().BeEmpty();
        new[] { 1, 2 }.NullToEmpty().Should().Equal(1, 2);
    }
}
