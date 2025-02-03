using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.GetAllUserName;

public class ExistWithUserNameHandler : IRequestHandler<ExistWithUserNameQuery, Result<bool>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public ExistWithUserNameHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<bool>> Handle(ExistWithUserNameQuery request, CancellationToken cancellationToken)
    {
        var user = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(u => u.UserName == request.UserName);
        if (user == null)
        {
            return Result.Ok(false);
        }

        return Result.Ok(true);
    }
}