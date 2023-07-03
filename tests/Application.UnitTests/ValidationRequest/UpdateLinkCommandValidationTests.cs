using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Commands.UpdateLink;

namespace Application.UnitTests.ValidationRequest;

public class UpdateLinkCommandValidationTests
{
    private readonly string _validUrl = "https://git.com";
    private readonly UpdateLinkCommandValidator _validator;
    private readonly Guid _linkId = Guid.NewGuid();

    public UpdateLinkCommandValidationTests()
    {
        _validator = new UpdateLinkCommandValidator();
    }


    public  static IEnumerable<object[]> ValidDataForValidation = new List<object[]>()
    {
        new object[] { "http://git.com", "goog" },
        new object[] { "https://myspace.com", "" },
        new object[] { "    ", "   " },
        new object[] { "", "" },
        new object[] { null!, null! },
        new object[] { null!, "aaa" }
    };

    [Theory]
    [MemberData(nameof(ValidDataForValidation))]
    public void Should_return_SuccessResult_When_dataIsValid(string url, string? alias)
    {
        //arrange
        var request = new UpdateLinkCommand(_linkId, url, alias);

        //act 
        var result = _validator.Validate(request);

        //assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("google.com")]
    [InlineData("www.yandex.com")]
    public void VShould_return_ErrorsResult_When_UrlIsInvalid(string url)
    {
        //arrange
        string? alias = null;
        var request = new UpdateLinkCommand(_linkId, url, alias);
        //act 
        var result = _validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.UrlAddressIsNotUrl, errors);
    }

    [Fact]
    public void Should_return_ErrorsResult_WhenIdIsDefault()
    {
        //arrange
        Guid badId = default;
        var url = "https://google.com";
        var alias = "goog";
        var request = new UpdateLinkCommand(badId, url, alias);

        //act 
        var result = _validator.Validate(request);

        //assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);

        var errors = result.Errors.Select(s => s.ErrorMessage).ToArray();
        Assert.Contains(LinkValidationErrorMessage.IdRequired, errors);
        Assert.Single(errors);
    }

    [Theory]
    [InlineData("1w")] // few letters < 3
    [InlineData("asdfgjgkgkdldudlfyfuljdnsjedfdfgfg")] // many letters > 30
    public void Should_return_ErrorsResult_When_AliasLengthIsInvalid(string alias)
    {
        //arrange
        var request = new UpdateLinkCommand(_linkId, _validUrl, alias);

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
        var request = new UpdateLinkCommand(_linkId, _validUrl, alias);

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