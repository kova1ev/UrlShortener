using Application.UnitTests.Utility;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Queries.GetLinkById;

namespace Application.UnitTests.Links.Queries;

public class GetByIdTests
{
    [Fact]
    public async Task Get_link_by_id_Success()
    {
        //arrange
        var id = new Guid("567BD1BF-6287-4331-A50E-82984DB0B97D");
        GetLinkByIdQuery request = new(id);

        using var context = DbContextHepler.CreateContext();
        GetLinkByIdQueryHandler handler = new(context);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors!);
        Assert.Equal(id, result.Value.Id);
    }

    [Fact]
    public async Task Get_link_by_badId_Failure()
    {
        //arrange
        var badId = new Guid("567BDaaa-6287-4331-A50E-82984DB0B97D");
        GetLinkByIdQuery request = new(badId);

        using var context = DbContextHepler.CreateContext();
        GetLinkByIdQueryHandler handler = new(context);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.NotEmpty(result.Errors!);
        Assert.Single(result.Errors);
        Assert.Equal(LinkValidationErrorMessage.LinkNotExisting, result.Errors.FirstOrDefault());
    }
}