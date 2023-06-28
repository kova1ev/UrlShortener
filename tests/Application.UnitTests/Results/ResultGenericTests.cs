using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;

namespace Application.UnitTests.Results;

public class ResultGenericTests
{
    [Fact]
    public void Result_T_Success()
    {
        // arrange
        var test = "test";

        // act
        var result = Result<string>.Success(test);

        // assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors!);
        Assert.Equal(test, result.Value);
    }

    [Fact]
    public void Result_T_Failure()
    {
        // arrange 
        string[] errors =
        {
            LinkValidationErrorMessage.AliasTaken,
            LinkValidationErrorMessage.IdRequired
        };

        // act
        var result = Result<string>.Failure(errors);

        // assert
        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.Throws<InvalidOperationException>(() => result.Value);
        Assert.Equal(2, result.Errors?.ToArray().Length);
        Assert.Equal(LinkValidationErrorMessage.AliasTaken, result.Errors.ToArray()[0]);
        Assert.Equal(LinkValidationErrorMessage.IdRequired, result.Errors.ToArray()[1]);
    }


    [Fact]
    public void Result_T_Errors_Exception()
    {
        // arrange
        string[]? errors = null;
        // act          
        // assert

        Assert.Throws<ArgumentNullException>(() => Result<string>.Failure(errors!));
    }
}