using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Commands.CreateLink;

namespace Application.UnitTests.ValidationRequest;

public class CreateLinkCommandValidationTests
{
    private readonly string _validUrl = "https://git.com";
    private readonly CreateLinkCommandValidator _validator;

    public CreateLinkCommandValidationTests()
    {
        _validator = new CreateLinkCommandValidator();
    }

    [Theory]
    [InlineData("http://git.com", "goog")]
    [InlineData("https://google.com", null)]
    [InlineData("https://myspace.com", "")]
    public void Should_return_SuccessResult_When_dataIsValid(string url, string? alias)
    {
        //arrange
        var request = new CreateLinkCommand(url, alias);
        
        //act 
        var result = _validator.Validate(request);

        //assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("google.com", "goog")]
    [InlineData("www.yandex.com", null)]
    public void Should_return_ErrorsResult_When_UrlIsInvalid(string url, string alias)
    {
        //arrange
        var request = new CreateLinkCommand(url, alias);
        
        //act 
        var result = _validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.UrlAddressIsNotUrl, errors);
        Assert.Single(errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void Should_return_ErrorsResult_When_UrlIsWhiteSpaceOrNull(string? url)
    {
        //arrange
        var alias = "goog";
        var request = new CreateLinkCommand(url!, alias);

        //act 
        var result = _validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.UrlAddressRequired, errors);
        Assert.Single(errors);
    }

    [Theory]
    [InlineData("1w")] // few letters < 3
    [InlineData("asdfgjgkgkdldudlfyfuljdnsjedfdfgfg")] // many letters > 30
    public void Should_return_ErrorsResult_When_AliasLengthIsInvalid(string alias)
    {
        //arrange
        var request = new CreateLinkCommand(_validUrl, alias);

        //act 
        var result = _validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.AliasBadRange, errors);
        Assert.Single(errors);
    }

    [Theory]
    [InlineData("fgg  df   gf")]
    [InlineData("fg gdf")]
    public void Should_return_ErrorsResult_WhenAliasContainWhiteSpace(string alias)
    {
        //arrange
        var request = new CreateLinkCommand(_validUrl, alias);

        //act
        var result = _validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.AliasHaveWhitespace, errors);
        Assert.Single(errors);
    }
}