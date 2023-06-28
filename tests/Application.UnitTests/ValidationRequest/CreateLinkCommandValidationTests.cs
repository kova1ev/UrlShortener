using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Commands.CreateLink;

namespace Application.UnitTests.ValidationRequest;

public class CreateLinkCommandValidationTests
{
    [Theory]
    [InlineData("http://git.com", "goog")]
    [InlineData("https://google.com", null)]
    [InlineData("https://myspace.com", "")]
    public void Validate_createLinkCommand_Success(string url, string? alias)
    {
        //arrange
        var request = new CreateLinkCommand(url, alias);
        var validator = new CreateLinkCommandValidator();
        //act 
        var result = validator.Validate(request);

        //assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("google.com")]
    [InlineData("www.yandex.com")]
    public void Validate_createLinkCommand_bad_url(string url)
    {
        //arrange
        var alias = "goog";
        var request = new CreateLinkCommand(url, alias);

        var validator = new CreateLinkCommandValidator();
        //act 
        var result = validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.UrlAddressIsNotUrl, errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_createLinkCommand_empty_url(string? url)
    {
        //arrange
        var alias = "goog";
        var request = new CreateLinkCommand(url, alias);
        var validator = new CreateLinkCommandValidator();

        //act 
        var result = validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.UrlAddressRequired, errors);
        Assert.True(errors.Length == 1);
    }

    [Theory]
    [InlineData("1w")] // few letters < 3
    [InlineData("asdfgjgkgkdldudlfyfuljdnsjedfdfgfg")] // many letters > 30
    public void Validate_createLinkCommand_with_bad_Alias(string alias)
    {
        //arrange
        var url = "https://git.com";
        var request = new CreateLinkCommand(url, alias);
        var validator = new CreateLinkCommandValidator();

        //act 
        var result = validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.AliasBadRange, errors);
        Assert.True(errors.Length == 1);
    }

    [Theory]
    [InlineData("fgg  df   gf")]
    [InlineData("fg gdf")]
    public void Validate_CreateLInkCommand_ShouldReturnFailureResult_with_WhiteSpaceError(string alias)
    {
        //arrange
        var url = "https://git.com";
        var request = new CreateLinkCommand(url, alias);
        var validator = new CreateLinkCommandValidator();

        //act
        var result = validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.AliasHaveWhitespace, errors);
        Assert.True(errors.Length == 1);
    }
}