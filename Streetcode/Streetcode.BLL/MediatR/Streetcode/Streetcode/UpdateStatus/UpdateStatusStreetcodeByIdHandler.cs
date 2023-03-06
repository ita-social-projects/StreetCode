using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateStatus;

public class UpdateStatusStreetcodeByIdHandler : IRequestHandler<UpdateStatusStreetcodeByIdCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public UpdateStatusStreetcodeByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
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

        _repositoryWrapper.StreetcodeRepository.Update(streetcode);

        var resultIsSuccessChangeStage = await _repositoryWrapper.SaveChangesAsync() > 0;

        if (!resultIsSuccessChangeStage)
        {
            throw new Exception("Failed to change streetcode stage!");
        }

        return Result.Ok(Unit.Value);
    }
}
