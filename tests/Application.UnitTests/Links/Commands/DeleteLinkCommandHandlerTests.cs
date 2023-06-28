using Application.UnitTests.Utility;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Links.Commands.DeleteLink;

namespace Application.UnitTests.Links.Commands;

public class DeleteLinkCommandHandlerTests
{
    [Fact]
    public async Task Delete_link_Success()
    {
        //arrange
        var linkId = new Guid("64DE08F3-B627-46EE-AE23-C2C873FC4C11");

        var request = new DeleteLinkCommand(linkId);
        using var context = DbContextHepler.CreateContext();
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
    public async Task Delete_link_with_bad_id_Failure()
    {
        //arrange
        Guid badId = default;

        var request = new DeleteLinkCommand(badId);
        using var context = DbContextHepler.CreateContext();
        var handler = new DeleteLinkCommandHandler(context);
        var iniLinksCount = await context.Links.CountAsync();

        //act
        var result = await handler.Handle(request, CancellationToken.None);

        //assert 
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors!);
        Assert.Single(result.Errors);
        Assert.Equal(LinkValidationErrorMessage.LinkNotExisting, result.Errors?.First());
        Assert.Equal(iniLinksCount, context.Links.Count());
    }
}