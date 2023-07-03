using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;

namespace Application.UnitTests.Results;

public class ResultTests
{
    [Fact]
    public void ResultSuccess_Should_return_Success()
    {
        // act
        var result = Result.Success();

        // assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ResultFailure_Should_return_Errors()
    {
        // arrange 
        string[] errors =
        {
            LinkValidationErrorMessage.AliasTaken,
            LinkValidationErrorMessage.IdRequired
        };

        // act
        var result = Result.Failure(errors);

        // assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errors.Length, result.Errors.ToArray().Length);
        Assert.Equal(LinkValidationErrorMessage.AliasTaken, result.Errors.ToArray()[0]);
        Assert.Equal(LinkValidationErrorMessage.IdRequired, result.Errors.ToArray()[1]);
    }

    [Fact]
    public void ResultFailure_Should_Throw_ArgumentNullException_when_Errors_Null()
    {
        // arrange
        string[]? errors = null;
        
        // assert
        Assert.Throws<ArgumentNullException>(() => Result.Failure(errors!));
    }
}