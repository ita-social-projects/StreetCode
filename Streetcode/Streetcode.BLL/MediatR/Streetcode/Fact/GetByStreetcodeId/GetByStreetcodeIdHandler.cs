using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;

public class GetByStreetcodeIdHandler : IRequestHandler<GetFactByStreetcodeIdQuery, Result<FactDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<FactDTO>> Handle(GetFactByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var fact = await _repositoryWrapper.FactRepository
            .GetSingleOrDefaultAsync(f => f.Streetcodes.Any(s => s.Id == request.StreetcodeId));

        if (fact is null)
        {
            return Result.Fail(new Error($"Cannot find a fact by a streetcode categoryId: {request.StreetcodeId}"));
        }

        var factDto = _mapper.Map<FactDTO>(fact);
        return Result.Ok(factDto);
    }
}