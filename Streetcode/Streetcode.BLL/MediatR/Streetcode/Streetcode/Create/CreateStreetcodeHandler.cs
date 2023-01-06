using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;

public class CreateStreetcodeHandler : IRequestHandler<CreateStreetcodeCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateStreetcodeHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(CreateStreetcodeCommand request, CancellationToken cancellationToken)
    {
        var streetcode = _mapper.Map<DAL.Entities.Streetcode.StreetcodeContent>(request.Streetcode);

        if (streetcode is null)
        {
            return Result.Fail(new Error("Cannot convert null to Streetcode"));
        }

        _repositoryWrapper.StreetcodeRepository.Create(streetcode);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to create a streetcode"));
    }
}