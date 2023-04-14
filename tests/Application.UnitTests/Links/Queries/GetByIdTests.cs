using Application.UnitTests.Utility;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Links.Queries.GetLinkById;
using UrlShortener.Data;

namespace Application.UnitTests.Links.Queries;

public class GetByIdTests
{

    [Fact]
    public async Task Get_link_by_id_Success()
    {
        //arrange
        Guid id = new Guid("567BD1BF-6287-4331-A50E-82984DB0B97D");
        GetLinkByIdQuery request = new(id);

        using AppDbContext context = DbContextCreator.CreateContext();
        GetLinkByIdQueryHandler handler = new(context);

        //act
        Result<LinkDetailsResponse> result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors!);
        Assert.Equal(result.Value.Id, id);
    }

    [Fact]
    public async Task Get_link_by_badId_Failure()
    {
        //arrange
        Guid badId = new Guid("567BDaaa-6287-4331-A50E-82984DB0B97D");
        GetLinkByIdQuery request = new(badId);

        using AppDbContext context = DbContextCreator.CreateContext();
        GetLinkByIdQueryHandler handler = new(context);

        //act
        Result<LinkDetailsResponse> result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.NotEmpty(result.Errors!);
        Assert.Equal(1, result.Errors.Count());
        Assert.Equal(LinkValidationErrorMessage.LINK_NOT_EXISTING, result.Errors.FirstOrDefault());
    }

}
