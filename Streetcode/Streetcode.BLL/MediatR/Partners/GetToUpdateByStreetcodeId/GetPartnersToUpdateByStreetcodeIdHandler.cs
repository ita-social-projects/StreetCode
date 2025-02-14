using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeIdToUpdate
{
    public class GetPartnersToUpdateByStreetcodeIdHandler : IRequestHandler<GetPartnersToUpdateByStreetcodeIdQuery, Result<IEnumerable<PartnerDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetPartnersToUpdateByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetPartnersToUpdateByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var partners = await _repositoryWrapper.PartnersRepository
                .GetAllAsync(
                    predicate: p => p.Streetcodes.Any(sc => sc.Id == request.StreetcodeId),
                    include: p => p.Include(pl => pl.PartnerSourceLinks));

            if (!partners.Any())
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindPartnersByStreetcodeId", request.StreetcodeId].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(value: _mapper.Map<IEnumerable<PartnerDTO>>(partners));
        }
    }
}
