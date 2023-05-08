using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;

public class GetFactByStreetcodeIdHandler : IRequestHandler<GetFactByStreetcodeIdQuery, Result<IEnumerable<FactDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetFactByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<FactDTO>>> Handle(GetFactByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var fact = await _repositoryWrapper.FactRepository
            .GetAllAsync(f => f.StreetcodeId == request.StreetcodeId);

        if (fact is null)
        {
            return Result.Fail(new Error($"Cannot find any fact by the streetcode id: {request.StreetcodeId}"));
        }

        var factDto = _mapper.Map<IEnumerable<FactDTO>>(fact);
        return Result.Ok(factDto);
    }
}