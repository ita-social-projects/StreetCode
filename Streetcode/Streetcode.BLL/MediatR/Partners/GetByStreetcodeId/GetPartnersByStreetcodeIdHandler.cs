using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeId;

public class GetPartnersByStreetcodeIdHandler : IRequestHandler<GetPartnersByStreetcodeIdQuery, Result<IEnumerable<PartnerDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetPartnersByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetPartnersByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetSingleOrDefaultAsync(st => st.Id == request.StreetcodeId);

        if (streetcode is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind?["CannotFindAnyStreetcodeWithCorrespondingStreetcodeId", request.StreetcodeId].Value));
        }

        var partners = await _repositoryWrapper.PartnersRepository
            .GetAllAsync(
                predicate: p => p.Streetcodes.Any(sc => sc.Id == streetcode.Id) || p.IsVisibleEverywhere,
                include: p => p.Include(pl => pl.PartnerSourceLinks));

        if (partners is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind?["CannotFindPartnersByStreetcodeId", request.StreetcodeId].Value));
        }

        var partnerDtos = _mapper.Map<IEnumerable<PartnerDTO>>(partners);
        return Result.Ok(value: partnerDtos);
    }
}
