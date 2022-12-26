using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.Queries;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Handlers.QueryHandlers;

public class GetAllFactsHandler : IRequestHandler<GetAllFactsQuery, Result<IEnumerable<FactDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllFactsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<FactDTO>>> Handle(GetAllFactsQuery request, CancellationToken cancellationToken)
    {
        var facts = await _repositoryWrapper.FactRepository.GetAllAsync();

        var factDtos = _mapper.Map<IEnumerable<FactDTO>>(facts);
        return Result.Ok(factDtos);
    }
}