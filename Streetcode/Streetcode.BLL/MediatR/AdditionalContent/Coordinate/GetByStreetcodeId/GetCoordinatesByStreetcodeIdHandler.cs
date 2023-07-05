using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId;

public class GetCoordinatesByStreetcodeIdHandler : IRequestHandler<GetCoordinatesByStreetcodeIdQuery, Result<IEnumerable<StreetcodeCoordinateDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetCoordinatesByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<StreetcodeCoordinateDTO>>> Handle(GetCoordinatesByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var coordinates = await _repositoryWrapper.StreetcodeCoordinateRepository
            .GetAllAsync(c => c.StreetcodeId == request.StreetcodeId);

        if (coordinates is null)
        {
            return Result.Fail(new Error(_stringLocalizer?["CannotFindCoordinates", request.StreetcodeId].Value));
        }

        var coordinatesDto = _mapper.Map<IEnumerable<StreetcodeCoordinateDTO>>(coordinates);
        return Result.Ok(coordinatesDto);
    }
}
