using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;

public class GetAllStreetcodesHandler : IRequestHandler<GetAllStreetcodesQuery, Result<IEnumerable<StreetcodeDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllStreetcodesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<StreetcodeDTO>>> Handle(GetAllStreetcodesQuery request, CancellationToken cancellationToken)
    {
        var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync();

        if (streetcodes is null)
        {
            return Result.Fail(new Error($"Cannot find any streetcode"));
        }

        var streetcodeDtos = _mapper.Map<IEnumerable<StreetcodeDTO>>(streetcodes);
        return Result.Ok(streetcodeDtos);
    }
}