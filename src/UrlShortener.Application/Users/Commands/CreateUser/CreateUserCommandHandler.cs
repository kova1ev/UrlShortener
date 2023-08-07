using MediatR;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Application.Common.Result;
using UrlShortener.Entity;

namespace UrlShortener.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
{
    private readonly UserManager<User> _userManager;

    public CreateUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User() { UserName = request.UserName, Email = request.Email };

        var result = await _userManager.CreateAsync(user, request.Password!);

        if (result.Succeeded == false)
        {
            return Result.Failure(result.Errors.Select(error => error.Description));
        }

        var roleAddResult = await _userManager.AddToRoleAsync(user, RoleConstant.User);
        if (roleAddResult.Succeeded == false)
        {
            throw new InvalidOperationException($"Role '{RoleConstant.User}' Not Added.");
        }

        return Result.Success();
    }
}