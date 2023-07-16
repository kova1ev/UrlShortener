using MediatR;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.Users.Queries;

public class GetValidUserQueryHandler : IRequestHandler<GetValidUserQuery, Result<User>>
{
    private readonly User _admin;

    public GetValidUserQueryHandler(IOptions<User> option)
    {
        _admin = option.Value ?? throw new ArgumentNullException(nameof(option));
    }

    public async Task<Result<User>> Handle(GetValidUserQuery request, CancellationToken cancellationToken)
    {
        // todo  get admin by email
        // validate admin password
        if (request.Email == _admin.Email && request.Password == _admin.Password)
        {
            return Result<User>.Success(_admin);
        }

        return Result<User>.Failure(new string[] { "Email or password is invalid." });
    }
}
