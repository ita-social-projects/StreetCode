using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.Create
{
    public class CreatePartnerHandler : IRequestHandler<CreatePartnerQuery, Result<PartnerDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
        private readonly IStringLocalizer<FieldNamesSharedResource> _stringLocalizerFieldNames;
        private readonly IStringLocalizer<AlreadyExistSharedResource> _stringLocalizerAlreadyExist;

        public CreatePartnerHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<NoSharedResource> stringLocalizerNo,
            IStringLocalizer<FieldNamesSharedResource> stringLocalizerFieldNames,
            IStringLocalizer<AlreadyExistSharedResource> stringLocalizerAlreadyExist)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
            _stringLocalizerFieldNames = stringLocalizerFieldNames;
            _stringLocalizerAlreadyExist = stringLocalizerAlreadyExist;
        }

        // If you use Rider instead of Visual Studio, for example, "SuppressMessage" attribute suppresses PossibleMultipleEnumeration warning
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Here is no sense to do materialization of query because of nested ToListAsync method in GetAllAsync method")]
        public async Task<Result<PartnerDTO>> Handle(CreatePartnerQuery request, CancellationToken cancellationToken)
        {
            var newPartner = _mapper.Map<Partner>(request.newPartner);
            try
            {
                var duplicateLogoPartner = await _repositoryWrapper.PartnersRepository
                    .GetFirstOrDefaultAsync(p => p.LogoId == request.newPartner.LogoId);

                if (duplicateLogoPartner is not null)
                {
                    string errorMsg = _stringLocalizerAlreadyExist["ConnectionAlreadyExist", _stringLocalizerFieldNames["LogoId"]].Value;
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(errorMsg);
                }

                newPartner.Streetcodes.Clear();

                var streetcodeIds = request.newPartner.Streetcodes.Select(s => s.Id).ToList();
                var existingStreetcodes = await _repositoryWrapper.StreetcodeRepository
                    .GetAllAsync(s => streetcodeIds.Contains(s.Id));

                var missingIds = streetcodeIds.Except(existingStreetcodes.Select(s => s.Id)).ToList();
                if (missingIds.Any())
                {
                    string errorMsg = _stringLocalizerNo["NoExistingStreetcodeWithId", string.Join(", ", missingIds)].Value;
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(errorMsg);
                }

                newPartner = await _repositoryWrapper.PartnersRepository.CreateAsync(newPartner);
                await _repositoryWrapper.SaveChangesAsync();

                foreach (var streetcode in existingStreetcodes)
                {
                    _repositoryWrapper.StreetcodeRepository.Attach(streetcode);
                }

                newPartner.Streetcodes.AddRange(existingStreetcodes);

                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(_mapper.Map<PartnerDTO>(newPartner));
            }
            catch(Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}
