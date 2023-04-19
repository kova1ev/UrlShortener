using FluentValidation.Results;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Commands.DeleteLink;

namespace Application.UnitTests.ValidationRequest;

public class DeleteLinkCommandValidationTests
{
    [Fact]
    public void Validate_deleteLinkCommand_Success()
    {
        var id = Guid.NewGuid();

        var request = new DeleteLinkCommand(id);
        var validator = new DeleteLinkCommandValidator();

        //act
        ValidationResult result = validator.Validate(request);

        //assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_deleteLinkCommand_without_id()
    {
        Guid id = default!;

        var request = new DeleteLinkCommand(id);
        var validator = new DeleteLinkCommandValidator();

        //act
        ValidationResult result = validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        Assert.Contains(LinkValidationErrorMessage.ID_REQUIRED, result.Errors.Select(e => e.ErrorMessage).ToArray());
    }
}
