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
    public class CreatePartnerHandler : IRequestHandler<CreatePartnerQuery, Result<PartnerDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public CreatePartnerHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        // If you use Rider instead of Visual Studio, for example, "SuppressMessage" attribute suppresses PossibleMultipleEnumeration warning
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Here is no sense to do materialization of query because of nested ToListAsync method in GetAllAsync method")]
        public async Task<Result<PartnerDto>> Handle(CreatePartnerQuery request, CancellationToken cancellationToken)
        {
            var newPartner = _mapper.Map<Partner>(request.newPartner);
            newPartner.Streetcodes.Clear();
            try
            {
                newPartner = await _repositoryWrapper.PartnersRepository.CreateAsync(newPartner);
                await _repositoryWrapper.SaveChangesAsync();

                var streetcodeIds = request.newPartner.Streetcodes.Select(s => s.Id).ToList();
                var existingStreetcodes = await _repositoryWrapper.StreetcodeRepository
                    .GetAllAsync(s => streetcodeIds.Contains(s.Id));

                foreach (var streetcode in existingStreetcodes)
                {
                    _repositoryWrapper.StreetcodeRepository.Attach(streetcode);
                }

                newPartner.Streetcodes.AddRange(existingStreetcodes);

                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(_mapper.Map<PartnerDto>(newPartner));
            }
            catch(Exception ex)
            {
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}
