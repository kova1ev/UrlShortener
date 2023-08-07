using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;
using UrlShortener.Entity;

namespace UrlShortener.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserResponse>>>
{
    private readonly UserManager<User> _userManager;

    public GetAllUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<User> users = await _userManager.Users.ToListAsync(cancellationToken);

        var result = users.Select(u => new UserResponse()
        {
            Id = u.Id,
            Name = u.UserName,
            Email = u.Email
        });
        return Result<IEnumerable<UserResponse>>.Success(result);
    }
}
