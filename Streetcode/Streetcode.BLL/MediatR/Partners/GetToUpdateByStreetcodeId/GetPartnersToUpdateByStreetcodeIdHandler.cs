using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetByStreetcodeIdToUpdate;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetToUpdateByStreetcodeId
{
    public class GetPartnersToUpdateByStreetcodeIdHandler : IRequestHandler<GetPartnersToUpdateByStreetcodeIdQuery, Result<IEnumerable<PartnerDto>>>
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

        public async Task<Result<IEnumerable<PartnerDto>>> Handle(GetPartnersToUpdateByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<StreetcodeContent, bool>>? basePredicate = str => str.Id == request.StreetcodeId;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

            var isStreetcodeExists = await _repositoryWrapper.StreetcodeRepository.FindAll(predicate: predicate).AnyAsync(cancellationToken);

            if (!isStreetcodeExists)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", request.StreetcodeId].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var partners = await _repositoryWrapper.PartnersRepository
                .GetAllAsync(
                    predicate: p => p.Streetcodes.Any(sc => sc.Id == request.StreetcodeId),
                    include: p => p.Include(pl => pl.PartnerSourceLinks));

            // even if there are no partners, we still want to return an empty enumerable
            if (!partners.Any())
            {
                string message = "Returning empty enumerable of partners to update";
                _logger.LogInformation(message);
                return Result.Ok(Enumerable.Empty<PartnerDto>());
            }

            return Result.Ok(value: _mapper.Map<IEnumerable<PartnerDto>>(partners));
        }
    }
}