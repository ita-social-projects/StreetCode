using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create;

public class CreateFactHandler : IRequestHandler<CreateFactCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateFactHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(CreateFactCommand request, CancellationToken cancellationToken)
    {
        var fact = _mapper.Map<DAL.Entities.Streetcode.TextContent.Fact>(request.Fact);

        if (fact is null)
        {
            return Result.Fail(new Error("Cannot convert null to Fact"));
        }

        _repositoryWrapper.FactRepository.Create(fact);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to create a fact"));
    }
}