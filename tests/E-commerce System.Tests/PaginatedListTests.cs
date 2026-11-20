using E_commerce_System.results;
using Xunit;

namespace E_commerce_System.Tests;

public class PaginatedListTests
{
    [Fact]
    public void NewList_UsesFirstPageAndDefaultPageSize()
    {
        var list = new PaginatedList<int>();

        Assert.Equal(1, list.PageNumber);
        Assert.Equal(10, list.PageSize);
    }

    [Fact]
    public void PageSize_AcceptsOne()
    {
        var list = new PaginatedList<int> { PageSize = 1 };

        Assert.Equal(1, list.PageSize);
    }

    [Fact]
    public void PageSize_AcceptsDefaultSize()
    {
        var list = new PaginatedList<int> { PageSize = 10 };

        Assert.Equal(10, list.PageSize);
    }

    [Fact]
    public void PageSize_AcceptsMaximumSize()
    {
        var list = new PaginatedList<int> { PageSize = 20 };

        Assert.Equal(20, list.PageSize);
    }

    [Fact]
    public void PageSize_ClampsValuesAboveMaximum()
    {
        var list = new PaginatedList<int> { PageSize = 21 };

        Assert.Equal(20, list.PageSize);
    }

    [Fact]
    public void PageSize_CanBeUpdatedMoreThanOnce()
    {
        var list = new PaginatedList<int> { PageSize = 5 };
        list.PageSize = 15;

        Assert.Equal(15, list.PageSize);
    }
}
