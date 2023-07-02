using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;

namespace Streetcode.BLL.MediatR.Partners.GetByStreetcodeIdToUpdate
{
    public class GetPartnersToUpdateByStreetcodeIdHandler : IRequestHandler<GetPartnersToUpdateByStreetcodeIdQuery, Result<IEnumerable<PartnerDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetPartnersToUpdateByStreetcodeIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetPartnersToUpdateByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository
                .GetSingleOrDefaultAsync(st => st.Id == request.StreetcodeId);

            if (streetcode is null)
            {
                string errorMsg = $"Cannot find any streetcode with corresponding streetcode id: {request.StreetcodeId}";
                _logger?.LogError("GetPartnersToUpdateByStreetcodeIdQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var partners = await _repositoryWrapper.PartnersRepository
                .GetAllAsync(
                    predicate: p => p.Streetcodes.Any(sc => sc.Id == streetcode.Id),
                    include: p => p.Include(pl => pl.PartnerSourceLinks));

            if (partners is null)
            {
                string errorMsg = $"Cannot find a partners by a streetcode id: {request.StreetcodeId}";
                _logger?.LogError("GetPartnersToUpdateByStreetcodeIdQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var partnerDtos = _mapper.Map<IEnumerable<PartnerDTO>>(partners);
            _logger?.LogInformation($"GetPartnersToUpdateByStreetcodeIdQuery handled successfully. Retrieved {partnerDtos.Count()} partners");
            return Result.Ok(value: partnerDtos);
        }
    }
}
