using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;

public class DeleteSoftStreetcodeHandler : IRequestHandler<DeleteSoftStreetcodeCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public DeleteSoftStreetcodeHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<Unit>> Handle(DeleteSoftStreetcodeCommand request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (streetcode is null)
        {
            throw new Exception($"Cannot find a streetcode with corresponding categoryId: {request.Id}");
        }

        streetcode.Status = DAL.Enums.StreetcodeStatus.Deleted;
        streetcode.UpdatedAt = DateTime.Now;

        _repositoryWrapper.StreetcodeRepository.Update(streetcode);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (!resultIsSuccess)
        {
            throw new Exception("Failed to delete a streetcode");
        }

        return Result.Ok(Unit.Value);
    }
}
