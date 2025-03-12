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

            // even if there are no partners, we still want to return an empty enumerable
            if (!partners.Any())
            {
                string message = "Returning empty enumerable of partners to update";
                _logger.LogInformation(message);
                return Result.Ok(Enumerable.Empty<PartnerDTO>());
            }

            return Result.Ok(value: _mapper.Map<IEnumerable<PartnerDTO>>(partners));
        }
    }
}
