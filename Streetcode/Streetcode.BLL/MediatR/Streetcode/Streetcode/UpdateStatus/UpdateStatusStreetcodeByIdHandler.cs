using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateStatus;

public class UpdateStatusStreetcodeByIdHandler : IRequestHandler<UpdateStatusStreetcodeByIdCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public UpdateStatusStreetcodeByIdHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(UpdateStatusStreetcodeByIdCommand request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetFirstOrDefaultAsync(x => x.Id == request.Id);

        if (streetcode is null)
        {
            string errorMsg = $"Cannot find any streetcode with corresponding id: {request.Id}";
            _logger?.LogError("UpdateStatusStreetcodeByIdCommand handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        streetcode.Status = request.Status;
        streetcode.UpdatedAt = DateTime.Now;

        _repositoryWrapper.StreetcodeRepository.Update(streetcode);

        var resultIsSuccessChangeStatus = await _repositoryWrapper.SaveChangesAsync() > 0;

        if(resultIsSuccessChangeStatus)
        {
            _logger?.LogInformation($"UpdateStatusStreetcodeByIdCommand handled successfully");
            return Result.Ok(Unit.Value);
        }
        else
        {
            const string errorMsg = "Failed to update status of streetcode";
            _logger?.LogError("UpdateStatusStreetcodeByIdCommand handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}
