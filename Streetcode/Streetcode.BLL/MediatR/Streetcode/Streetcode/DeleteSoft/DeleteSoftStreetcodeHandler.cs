using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;

public class DeleteSoftStreetcodeHandler : IRequestHandler<DeleteSoftStreetcodeCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public DeleteSoftStreetcodeHandler(IRepositoryWrapper repositoryWrapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteSoftStreetcodeCommand request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (streetcode is null)
        {
            string errorMsg = $"Cannot find a streetcode with corresponding categoryId: {request.Id}";
            _logger.LogError(request, errorMsg);
            throw new ArgumentNullException(errorMsg);
        }

        streetcode.Status = DAL.Enums.StreetcodeStatus.Deleted;
        streetcode.UpdatedAt = DateTime.Now;

        _repositoryWrapper.StreetcodeRepository.Update(streetcode);

        var resultIsDeleteSucces = await _repositoryWrapper.SaveChangesAsync() > 0;

        if(resultIsDeleteSucces)
        {
            return Result.Ok(Unit.Value);
        }
        else
        {
            const string errorMsg = "Failed to change status of streetcode to deleted";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }
    }
}