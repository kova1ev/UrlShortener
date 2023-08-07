using MediatR;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Result>
{
    public Guid Id { get; }

    public DeleteUserCommand(Guid id)
    {
        Id = id;
    }
}