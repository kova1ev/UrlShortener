using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Commands.DeleteLink;

namespace Application.UnitTests.ValidationRequest;

public class DeleteLinkCommandValidationTests
{
    private readonly DeleteLinkCommandValidator _validator;

    public DeleteLinkCommandValidationTests()
    {
        _validator = new DeleteLinkCommandValidator();
    }

    [Fact]
    public void Should_Return_SuccessResult()
    {
        var id = Guid.NewGuid();
        var request = new DeleteLinkCommand(id);

        //act
        var result = _validator.Validate(request);

        //assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Return_ErrorsResult_WhenId_isDefault()
    {
        Guid id = default;
        var request = new DeleteLinkCommand(id);

        //act
        var result = _validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(e => e.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.IdRequired, errors);
        Assert.Single(errors);
    }
}