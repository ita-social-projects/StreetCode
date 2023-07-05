using FluentResults;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;

public class DeleteStreetcodeHandler : IRequestHandler<DeleteStreetcodeCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public DeleteStreetcodeHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
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
            string errorMsg = $"Cannot find a streetcode with corresponding categoryId: {request.Id}";
            _logger.LogError($"DeleteStreetcodeCommand handled with an error. {errorMsg}");
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.StreetcodeRepository.Delete(streetcode);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        if(resultIsSuccess)
        {
            return Result.Ok(Unit.Value);
        }
        else
        {
            const string errorMsg = "Failed to delete a streetcode";
            _logger.LogError($"DeleteStreetcodeCommand handled with an error. {errorMsg}");
            return Result.Fail(new Error(errorMsg));
        }
    }
}