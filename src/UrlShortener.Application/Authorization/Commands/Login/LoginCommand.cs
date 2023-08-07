using MediatR;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Authorization.Commands.Login;

public class LoginCommand : IRequest<Result<AuthResponse>>
{
    public LoginCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; } 
    public string Password { get; }
}