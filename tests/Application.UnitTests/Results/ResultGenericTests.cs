using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;

namespace Application.UnitTests.Results;

public class ResultGenericTests
{
    [Fact]
    public void Result_T_Success()
    {
        // arrange
        string test = "test";

        // act
        Result<string> result = Result<string>.Success(test);

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
        string[] errors = new[] {
                LinkValidationErrorMessage.ALIAS_TAKEN,
                LinkValidationErrorMessage.ID_REQUIRED
            };

        // act
        Result<string> result = Result<string>.Failure(errors);

        // assert
        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.Throws<InvalidOperationException>(() => result.Value);
        Assert.Equal(2, result.Errors?.ToArray().Length);
        Assert.Equal(LinkValidationErrorMessage.ALIAS_TAKEN, result.Errors.ToArray()[0]);
        Assert.Equal(LinkValidationErrorMessage.ID_REQUIRED, result.Errors.ToArray()[1]);
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
