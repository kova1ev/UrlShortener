using MediatR;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Users.Queries;

public class GetValidUserQuery : IRequest<Result<User>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public GetValidUserQuery(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
