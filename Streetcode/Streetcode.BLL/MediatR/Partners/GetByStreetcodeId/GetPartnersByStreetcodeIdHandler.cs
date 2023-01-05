using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
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
        var streetcode = _repositoryWrapper.StreetcodeRepository.GetSingleOrDefaultAsync(st => st.Id == request.StreetcodeId).Result;

        if (streetcode is null)
        {
            return Result.Fail(new Error($"Cannot streetcode a fact with corresponding Id: {request.StreetcodeId}"));
        }

        var partners = streetcode.StreetcodePartners.Select(sp => sp.Partner);

        var partnerDtos = _mapper.Map<IEnumerable<PartnerDTO>>(partners);
        return Result.Ok(value: partnerDtos);
    }
}