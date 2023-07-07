using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeIdToUpdate
{
    public class GetPartnersToUpdateByStreetcodeIdHandler : IRequestHandler<GetPartnersToUpdateByStreetcodeIdQuery, Result<IEnumerable<PartnerDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetPartnersToUpdateByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetPartnersToUpdateByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository
                .GetSingleOrDefaultAsync(st => st.Id == request.StreetcodeId);

            if (streetcode is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingStreetcodeId", request.StreetcodeId].Value));
            }

            var partners = await _repositoryWrapper.PartnersRepository
                .GetAllAsync(
                    predicate: p => p.Streetcodes.Any(sc => sc.Id == streetcode.Id),
                    include: p => p.Include(pl => pl.PartnerSourceLinks));

            if (partners is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindPartnersByStreetcodeId", request.StreetcodeId].Value));
            }

            var partnerDtos = _mapper.Map<IEnumerable<PartnerDTO>>(partners);
            return Result.Ok(value: partnerDtos);
        }
    }
}
