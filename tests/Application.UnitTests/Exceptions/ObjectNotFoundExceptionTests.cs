using UrlShortener.Application.Common.Exceptions;

namespace Application.UnitTests.Exceptions;

public class ObjectNotFoundExceptionTests
{
    [Fact]
    public void Should_throw_Exception()
    {
        var action = new Func<object>(() => throw new ObjectNotFoundException());

        var exception = Assert.Throws<ObjectNotFoundException>(action);
        Assert.NotNull(exception.Message);
    }

    [Fact]
    public void Should_throw_Exception_with_CustomMessage()
    {
        var message = "some object not found!!!";

        var action = new Func<object>(() => throw new ObjectNotFoundException(message));

        var exception = Assert.Throws<ObjectNotFoundException>(action);
        Assert.NotNull(exception.Message);
        Assert.Equal(message, exception.Message);
    }
}