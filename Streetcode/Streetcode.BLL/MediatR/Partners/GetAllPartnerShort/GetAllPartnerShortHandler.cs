using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetAllPartnerShort
{
    internal class GetAllPartnerShortHandler : IRequestHandler<GetAllPartnersShortQuery, Result<IEnumerable<PartnerShortDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService? _logger;

        public GetAllPartnerShortHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<PartnerShortDTO>>> Handle(GetAllPartnersShortQuery request, CancellationToken cancellationToken)
        {
            var partners = await _repositoryWrapper.PartnersRepository.GetAllAsync();

            if (partners is null)
            {
                const string errorMsg = $"Cannot find any partners";
                _logger?.LogError("GetAllPartnersShortQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _logger?.LogInformation($"GetAllPartnersShortQuery handled successfully");
            return Result.Ok(_mapper.Map<IEnumerable<PartnerShortDTO>>(partners));
        }
    }
}
