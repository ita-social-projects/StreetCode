using System.Linq;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeId;

public class GetPartnersByStreetcodeIdHandler : IRequestHandler<GetPartnersByStreetcodeIdQuery, Result<IEnumerable<PartnerDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetPartnersByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetPartnersByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository
            .GetSingleOrDefaultAsync(st => st.Id == request.streetcodeId);

        if (streetcode is null)
        {
            return Result.Fail(new Error($"Cannot find a fact with corresponding streetcode id: {request.streetcodeId}"));
        }

        var partners = await _repositoryWrapper.PartnersRepository
            .GetAllAsync(
                predicate: p => p.Streetcodes.Any(sc => sc.Id == streetcode.Id) || p.IsKeyPartner,
                include: p => p.Include(pl => pl.PartnerSourceLinks));

        var partnerDtos = _mapper.Map<IEnumerable<PartnerDTO>>(partners);
        return Result.Ok(value: partnerDtos);
    }
}