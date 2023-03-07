using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;

namespace Application.UnitTests.Results
{
    public class ResultTests
    {
        [Fact]
        public void Result_Success()
        {
            // arrange
            // act
            Result result = Result.Success();

            // assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors!);
        }

        [Fact]
        public void Result_Failure()
        {
            // arrange 
            string[] errors = new[] {
                LinkValidationErrorMessage.ALIAS_TAKEN,
                LinkValidationErrorMessage.ID_REQUIRED
            };

            // act
            Result result = Result.Failure(errors);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Equal(2, result.Errors?.ToArray().Length);
            Assert.Equal(LinkValidationErrorMessage.ALIAS_TAKEN, result.Errors.ToArray()[0]);
            Assert.Equal(LinkValidationErrorMessage.ID_REQUIRED, result.Errors.ToArray()[1]);
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
}