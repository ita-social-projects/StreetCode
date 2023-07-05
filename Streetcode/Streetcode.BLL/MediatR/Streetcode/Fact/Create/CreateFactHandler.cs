using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Create;

public class CreateFactHandler : IRequestHandler<CreateFactCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public CreateFactHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(CreateFactCommand request, CancellationToken cancellationToken)
    {
        var fact = _mapper.Map<DAL.Entities.Streetcode.TextContent.Fact>(request.Fact);

        if (fact is null)
        {
            const string errorMsg = "Cannot convert null to Fact";
            _logger.LogError($"CreateFactCommand handled with an error. {errorMsg}");
            return Result.Fail(new Error(errorMsg));
        }

        _repositoryWrapper.FactRepository.Create(fact);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        if (resultIsSuccess)
        {
            return Result.Ok(Unit.Value);
        }
        else
        {
            const string errorMsg = "Failed to create a fact";
            _logger.LogError($"CreateFactCommand handled with an error. {errorMsg}");
            return Result.Fail(new Error(errorMsg));
        }
    }
}