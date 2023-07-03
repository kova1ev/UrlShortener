using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Commands.DeleteLink;

namespace Application.UnitTests.Links.Commands;

public class DeleteLinkCommandHandlerTests
{
    [Fact]
    public async Task DeleteLink_Should_return_SuccessResult()
    {
        //arrange
        var link = SeedData.Links.Last();
        var request = new DeleteLinkCommand(link.Id);
        
        using var context = DbContextHelper.CreateContext();
        var handler = new DeleteLinkCommandHandler(context);
        var iniLinksCount = await context.Links.CountAsync();

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert 
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotEqual(iniLinksCount, context.Links.Count());
    }

    [Fact]
    public async Task DeleteLink_Should_return_FailureResult_WhenIdIsInvalid()
    {
        //arrange
        Guid badId = default;
        var request = new DeleteLinkCommand(badId);
        
        using var context = DbContextHelper.CreateContext();
        var handler = new DeleteLinkCommandHandler(context);
        var iniLinksCount = await context.Links.CountAsync();

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert 
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal(LinkValidationErrorMessage.LinkNotExisting, result.Errors.First());
        Assert.Equal(iniLinksCount, context.Links.Count());
    }
}