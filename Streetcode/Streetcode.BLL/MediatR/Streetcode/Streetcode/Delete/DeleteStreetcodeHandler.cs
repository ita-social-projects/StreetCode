using FluentResults;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;

public class DeleteStreetcodeHandler : IRequestHandler<DeleteStreetcodeCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public DeleteStreetcodeHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<Unit>> Handle(DeleteStreetcodeCommand request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetFirstOrDefaultAsync(
            predicate: s => s.Id == request.Id,
            include: s => s.Include(x => x.Observers)
                           .Include(x => x.Targets));

        if (streetcode is null)
        {
            return Result.Fail(new Error($"Cannot find a streetcode with corresponding categoryId: {request.Id}"));
        }

        _repositoryWrapper.StreetcodeRepository.Delete(streetcode);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (!resultIsSuccess)
        {
            return Result.Fail("Failed to delete a streetcode");
        }

        return Result.Ok(Unit.Value);
    }
}