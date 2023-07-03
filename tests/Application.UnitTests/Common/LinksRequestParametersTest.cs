using UrlShortener.Application.Common.Domain.Links;

namespace Application.UnitTests.Common;

public class LinksRequestParametersTest
{
    [Fact]
    public void Should_Create_LinksRequestParameters_When_ParametersIsValid()
    {
        //arrange
        int currentPage = 2;
        int pageSize = 10;
        //act
        var linksRequestParameters = new LinksRequestParameters
        {
            Page = currentPage,
            PageSize = pageSize
        };

        //assert 
        Assert.Equal(currentPage, linksRequestParameters.Page);
        Assert.Equal(pageSize, linksRequestParameters.PageSize);
        Assert.True(linksRequestParameters.Text == null);
        Assert.True(linksRequestParameters.DateSort == DateSort.Desc);
    }

    [Theory]
    [InlineData(-11,-122)]
    [InlineData(0,10000)]
    public void Should_Create_LinksRequestParameters_withDefaultParam_WhenInputParamsInInvalid(int page,int pageSize)
    {
        //arrange
        var expectedDefaultPage = 1;
        var expectedDefaultPageSize = 10;

        //act
        var linksRequestParameters = new LinksRequestParameters
        {
            Page = page,
            PageSize = pageSize
        };

        //assert 
        Assert.NotEqual(page, linksRequestParameters.Page);
        Assert.NotEqual(pageSize, linksRequestParameters.PageSize);

        Assert.Equal(expectedDefaultPage, linksRequestParameters.Page);
        Assert.Equal(expectedDefaultPageSize, linksRequestParameters.PageSize);

        Assert.True(linksRequestParameters.Text == null);
        Assert.True(linksRequestParameters.DateSort == DateSort.Desc);
    }
}