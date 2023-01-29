using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.MediatR.Toponyms.Create;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create;

public class CreateToponymHandler : IRequestHandler<CreateToponymQuery, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateToponymHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(CreateToponymQuery request, CancellationToken cancellationToken)
    {
        var toponym = _mapper.Map<DAL.Entities.Toponyms.Toponym>(request.Toponym);

        if (toponym is null)
        {
            return Result.Fail(new Error("Cannot convert null to Toponym"));
        }

        _repositoryWrapper.ToponymRepository.Create(toponym);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to create a toponym"));
    }
}