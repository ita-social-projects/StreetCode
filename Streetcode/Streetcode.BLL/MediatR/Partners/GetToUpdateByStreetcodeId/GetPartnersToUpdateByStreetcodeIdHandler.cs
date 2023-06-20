using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeIdToUpdate
{
    public class GetPartnersToUpdateByStreetcodeIdHandler : IRequestHandler<GetPartnersToUpdateByStreetcodeIdQuery, Result<IEnumerable<PartnerDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetPartnersToUpdateByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetPartnersToUpdateByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository
                .GetSingleOrDefaultAsync(st => st.Id == request.StreetcodeId);

            if (streetcode is null)
            {
                return Result.Fail(new Error($"Cannot find any streetcode with corresponding streetcode id: {request.StreetcodeId}"));
            }

            var partners = await _repositoryWrapper.PartnersRepository
                .GetAllAsync(
                    predicate: p => p.Streetcodes.Any(sc => sc.Id == streetcode.Id),
                    include: p => p.Include(pl => pl.PartnerSourceLinks));

            if (partners is null)
            {
                return Result.Fail(new Error($"Cannot find a partners by a streetcode id: {request.StreetcodeId}"));
            }

            var partnerDtos = _mapper.Map<IEnumerable<PartnerDTO>>(partners);
            return Result.Ok(value: partnerDtos);
        }
    }
}
