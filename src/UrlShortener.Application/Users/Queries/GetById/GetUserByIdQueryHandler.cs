using MediatR;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;
using UrlShortener.Entity;

namespace UrlShortener.Application.Users.Queries.GetById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Result<UserResponse>.Failure(new[] { UserErrorMessage.UserNotFound });
        }

        return Result<UserResponse>.Success(new UserResponse()
            { Id = user.Id, Name = user.UserName, Email = user.Email });
    }
}