using UrlShortener.Application.Common.Models.Links;

namespace Application.UnitTests.Common;

public class LinksRequestParametersTest
{
    [Fact]
    public void Create_LinksRequestParameters_with_Valid_Parameters()
    {
        //arrange
        //act
        LinksRequestParameters linksRequestParameters = new LinksRequestParameters()
        {
            Page = 2,
            PageSize = 20
        };

        //assert 
        Assert.Equal(2, linksRequestParameters.Page);
        Assert.Equal(20, linksRequestParameters.PageSize);
        Assert.True(linksRequestParameters.Text == null);
        Assert.True(linksRequestParameters.DateSort == DateSort.Desc);
    }

    [Theory]
    [InlineData(-122)]
    [InlineData(10000)]
    public void Create_LinksRequestParameters_with_Invalid_Parameters(int pageSize)
    {
        //arrange
        int invalidPage = -10;
        int defaultPage = 1;
        int defaultPageSize = 10;

        //act
        LinksRequestParameters linksRequestParameters = new LinksRequestParameters()
        {
            Page = invalidPage,
            PageSize = pageSize
        };

        //assert 
        Assert.NotEqual(invalidPage, linksRequestParameters.Page);
        Assert.NotEqual(pageSize, linksRequestParameters.PageSize);

        Assert.Equal(defaultPage, linksRequestParameters.Page);
        Assert.Equal(defaultPageSize, linksRequestParameters.PageSize);

        Assert.True(linksRequestParameters.Text == null);
        Assert.True(linksRequestParameters.DateSort == DateSort.Desc);
    }
}
