using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateStatus;

public class UpdateStatusStreetcodeByIdHandler : IRequestHandler<UpdateStatusStreetcodeByIdCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public UpdateStatusStreetcodeByIdHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<Unit>> Handle(UpdateStatusStreetcodeByIdCommand request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetFirstOrDefaultAsync(x => x.Id == request.Id);

        if (streetcode is null)
        {
            return Result.Fail(new Error($"Cannot find any streetcode with corresponding id: {request.Id}"));
        }

        streetcode.Status = request.Status;
        streetcode.UpdatedAt = DateTime.Now;

        _repositoryWrapper.StreetcodeRepository.Update(streetcode);

        var resultIsSuccessChangeStatus = await _repositoryWrapper.SaveChangesAsync() > 0;

        return resultIsSuccessChangeStatus ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to update status of streetcode"));
    }
}
