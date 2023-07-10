using UrlShortener.Application.Common.Exceptions;

namespace Application.UnitTests.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void Should_throw_Exception()
    {
        var action = () => { throw new ValidationException(); };
        Assert.Throws<ValidationException>(action);
    }

    [Fact]
    public void Should_throw_Exception_WithCustomMessageAndErrorsList()
    {
        var message = "new errors";
        var errors = new List<string>() { "first", "second" };

        var action = () => { throw new ValidationException(message, errors); };

        Assert.Throws<ValidationException>(action);
    }

    [Fact]
    public void Should_throw_Exception_WithErrorsList()
    {
        var errors = new List<string>() { "first", "second" };

        var action = () => { throw new ValidationException(errors); };

        Assert.Throws<ValidationException>(action);
    }
}