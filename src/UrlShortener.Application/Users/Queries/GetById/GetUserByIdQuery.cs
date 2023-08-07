using MediatR;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;
using UrlShortener.Entity;

namespace UrlShortener.Application.Users.Queries.GetById;

public class GetUserByIdQuery : IRequest<Result<UserResponse>>
{
    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}