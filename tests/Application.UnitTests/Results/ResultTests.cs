using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;

namespace Application.UnitTests.Results;

public class ResultTests
{
    [Fact]
    public void Result_Success()
    {
        // arrange
        // act
        var result = Result.Success();

        // assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors!);
    }

    [Fact]
    public void Result_Failure()
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
        Assert.Equal(2, result.Errors?.ToArray().Length);
        Assert.Equal(LinkValidationErrorMessage.AliasTaken, result.Errors.ToArray()[0]);
        Assert.Equal(LinkValidationErrorMessage.IdRequired, result.Errors.ToArray()[1]);
    }

    [Fact]
    public void Result_Errors_Exception()
    {
        // arrange
        string[]? errors = null;
        // act          
        // assert

        Assert.Throws<ArgumentNullException>(() => Result.Failure(errors!));
    }
}