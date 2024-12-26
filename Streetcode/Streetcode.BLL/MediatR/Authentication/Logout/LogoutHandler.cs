using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Persistence;

namespace Streetcode.BLL.MediatR.Authentication.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly StreetcodeDbContext _dbContext;

    public LogoutHandler(StreetcodeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Fail("User not found.");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}