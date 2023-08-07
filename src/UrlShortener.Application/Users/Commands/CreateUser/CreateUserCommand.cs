using MediatR;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<Result>
{
    public CreateUserCommand(string userName, string email, string password)
    {
        UserName = userName;
        Email = email;
        Password = password;
    }

    public string UserName { get; }
    public string Email { get; }
    public string Password { get; }
}