using System.Text;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;

namespace Application.UnitTests.Results;

public class ResultGenericTests
{
    [Theory]
    [InlineData("test1")]
    [InlineData(123)]
    [InlineData(typeof(StringBuilder))]
    public void Result_T_Should_return_Success<T>(T value)
    {
        // act
        var result = Result<T>.Success(value);

        // assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Result_T_Success_Should_throw_ArgumentNullException_WhenValueIsNull()
    {
        // arrange
        string? data = null;
        
        // assert
        Assert.Throws<ArgumentNullException>(() => Result<string>.Success(data!));
    }
    
    [Fact]
    public void Result_T_Should_return_Errors()
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
        Assert.Equal(errors.Length, result.Errors.ToArray().Length);
        Assert.Equal(LinkValidationErrorMessage.AliasTaken, result.Errors.ToArray()[0]);
        Assert.Equal(LinkValidationErrorMessage.IdRequired, result.Errors.ToArray()[1]);
    }
    
    [Fact]
    public void Result_T_Failure_Should_throw_ArgumentNullException_WhenErrorsIsNull()
    {
        // arrange
        string[]? errors = null;
       
        // assert
        Assert.Throws<ArgumentNullException>(() => Result<string>.Failure(errors!));
    }
}