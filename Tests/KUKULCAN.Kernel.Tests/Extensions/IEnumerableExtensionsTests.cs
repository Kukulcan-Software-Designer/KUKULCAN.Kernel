using System;
using System.Collections.Generic;
using FluentAssertions;
using KUKULCAN.Kernel.Extensions;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Extensions;

public class IEnumerableExtensionsTests
{
    private class ProbeClass
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    [Fact]
    public void ToDataTable_CreatesTableWithColumnsAndRows()
    {
        var list = new List<ProbeClass>
        {
            new ProbeClass
            {
                Name = "Alice",
                Age = 30
            },
            new ProbeClass
            {
                Name = "Bob",
                Age = 40
            }
        };
        var table = list.ToDataTable();

        table.Columns.Contains("Name").Should().BeTrue();
        table.Columns.Contains("Age").Should().BeTrue();
        table.Rows.Count.Should().Be(2);
        table.Rows[0]["Name"].Should().Be("Alice");
    }

    [Fact]
    public void ToDataTable_UsesDbNull_ForNullPropertyValues()
    {
        var list = new List<ProbeClass>
        {
            new ProbeClass
            {
                Name = null!,
                Age = 30
            }
        };

        var table = list.ToDataTable();

        table.Rows[0]["Name"].Should().Be(DBNull.Value);
    }

    [Fact]
    public void ToDataTable_ThrowsOnNullSource()
    {
        IEnumerable<ProbeClass>? list = null;

        Action act = () => list!.ToDataTable();

        act.Should().Throw<ArgumentNullException>();
    }
}
