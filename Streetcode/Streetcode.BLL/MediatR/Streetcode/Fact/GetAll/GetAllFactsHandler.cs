using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;

public class GetAllFactsHandler : IRequestHandler<GetAllFactsQuery, Result<IEnumerable<FactDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _loggerService;

    public GetAllFactsHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService loggerService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _loggerService = loggerService;
    }

    public async Task<Result<IEnumerable<FactDTO>>> Handle(GetAllFactsQuery request, CancellationToken cancellationToken)
    {
        _loggerService.LogInformation("Entry into GetAllFactsHandler");

        var facts = await _repositoryWrapper.FactRepository.GetAllAsync();

        if (facts is null)
        {
            return Result.Fail(new Error($"Cannot find any fact"));
        }

        var factDtos = _mapper.Map<IEnumerable<FactDTO>>(facts);
        return Result.Ok(factDtos);
    }
}