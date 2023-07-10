using Application.UnitTests.Utility;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Queries.GetLinkById;
using UrlShortener.Entity;

namespace Application.UnitTests.Links.Queries;

public class GetByIdTests
{
    private readonly Link _seedLink = SeedData.Links.First();
    [Fact]
    public async Task GetById_Should_return_Link()
    {
        //arrange
        GetLinkByIdQuery request = new(_seedLink.Id);

        using var context = DbContextHelper.CreateContext();
        GetLinkByIdQueryHandler handler = new(context);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
        Assert.True(result.HasValue);
        Assert.Empty(result.Errors);
        Assert.Equal(_seedLink.Id, result.Value.Id);
    }

    [Fact]
    public async Task GetById_Should_return_FailureResult_When_IdIsBed()
    {
        //arrange
        var badId = new Guid("567BDaaa-6287-4331-A50E-82984DB0B97D");
        // or default id ;
        GetLinkByIdQuery request = new(badId);

        using var context = DbContextHelper.CreateContext();
        GetLinkByIdQueryHandler handler = new(context);

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert
        Assert.False(result.IsSuccess);
        Assert.False(result.HasValue);
        Assert.NotEmpty(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal(LinkValidationErrorMessage.LinkNotExisting, result.Errors.FirstOrDefault());
    }
}