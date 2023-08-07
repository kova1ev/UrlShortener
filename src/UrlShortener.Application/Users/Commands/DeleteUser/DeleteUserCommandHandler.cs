using MediatR;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Entity;

namespace UrlShortener.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly UserManager<User> _userManager;

    public DeleteUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
        {
            return Result.Failure(new[] { UserErrorMessage.UserNotFound });
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded == false)
        {
            return Result.Failure(result.Errors.Select(error => error.Description));
        }

        return Result.Success();
    }
}