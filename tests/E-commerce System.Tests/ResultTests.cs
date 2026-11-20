using E_commerce_System.results;
using Xunit;

namespace E_commerce_System.Tests;

public class ResultTests
{
    [Fact]
    public void Success_ReturnsSuccessfulResultWithoutError()
    {
        var result = Result.Succcess();

        Assert.True(result.IsSuccesse);
        Assert.False(result.IsFaliure);
        Assert.Empty(result.Erorr);
    }

    [Fact]
    public void Failure_ReturnsFailureWithErrorMessage()
    {
        var result = Result.Faliure("Request failed");

        Assert.False(result.IsSuccesse);
        Assert.True(result.IsFaliure);
        Assert.Equal("Request failed", result.Erorr);
    }

    [Fact]
    public void GenericSuccess_ReturnsValueAndSuccessfulStatus()
    {
        var result = GenericResult<int>.Success(42);

        Assert.True(result.IsSuccesse);
        Assert.Equal(42, result.Value);
        Assert.Empty(result.Erorr);
    }

    [Fact]
    public void GenericFailure_ReturnsErrorAndDefaultValue()
    {
        var result = GenericResult<int>.Faliuar("Not found");

        Assert.True(result.IsFaliure);
        Assert.Equal("Not found", result.Erorr);
        Assert.Equal(0, result.Value);
    }
}
