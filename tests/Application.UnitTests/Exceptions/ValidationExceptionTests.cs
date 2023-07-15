using UrlShortener.Application.Common.Exceptions;

namespace Application.UnitTests.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void Should_throw_Exception()
    {
        var action = new Action(() => throw new ValidationException());
        Assert.Throws<ValidationException>(action);
    }

    [Fact]
    public void Should_throw_Exception_WithCustomMessageAndErrorsList()
    {
        var message = "new errors";
        var errors = new List<string>() { "first", "second" };

        var action = new Action(() => throw new ValidationException(message, errors));

        var exception = Assert.Throws<ValidationException>(action);
        Assert.NotNull(exception.Message);
        Assert.Equal(message, exception.Message);

        Assert.NotNull(exception.Errors);
        Assert.Equal(errors, exception.Errors);
    }

    [Fact]
    public void Should_throw_Exception_WithErrorsList()
    {
        var errors = new List<string>() { "first", "second" };

        var action = new Action(() => throw new ValidationException(errors));

        var exception = Assert.Throws<ValidationException>(action);
        Assert.NotNull(exception.Errors);
        Assert.Equal(errors, exception.Errors);
    }
}