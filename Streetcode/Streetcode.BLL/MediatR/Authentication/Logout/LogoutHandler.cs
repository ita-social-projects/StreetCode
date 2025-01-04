using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Authentication.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public LogoutHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user == null)
        {
            return Result.Fail("User not found.");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        _repositoryWrapper.UserRepository.Update(user);
        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (resultIsSuccess)
        {
            return Result.Ok();
        }
        else
        {
            string errorMsg = "Failed to logout";
            return Result.Fail(new Error(errorMsg));
        }
    }
}