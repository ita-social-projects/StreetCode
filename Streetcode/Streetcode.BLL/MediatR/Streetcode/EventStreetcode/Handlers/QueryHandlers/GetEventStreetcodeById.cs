using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Queries;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Handlers.QueryHandlers;

public class GetEventStreetcodeByIdQueryHandler : IRequestHandler<GetEventStreetcodeByIdQuery, Result<EventStreetcodeDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetEventStreetcodeByIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<EventStreetcodeDTO>> Handle(GetEventStreetcodeByIdQuery request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.EventStreetcodeRepository.GetSingleOrDefaultAsync(st => st.Id == request.id);

        var streetcodeDto = _mapper.Map<EventStreetcodeDTO>(streetcode);
        return Result.Ok(streetcodeDto);
    }
}