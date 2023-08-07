using MediatR;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;
using UrlShortener.Entity;

namespace UrlShortener.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<Result<IEnumerable<UserResponse>>>
{
    
}