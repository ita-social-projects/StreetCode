using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.Create
{
    public class CreatePartnerHandler : IRequestHandler<CreatePartnerQuery, Result<PartnerDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public CreatePartnerHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PartnerDTO>> Handle(CreatePartnerQuery request, CancellationToken cancellationToken)
        {
            var newPartner = _mapper.Map<Partner>(request.newPartner);
            try
            {
                newPartner.Streetcodes.Clear();
                newPartner = await _repositoryWrapper.PartnersRepository.CreateAsync(newPartner);
                _repositoryWrapper.SaveChanges();
                var streetcodeIds = request.newPartner.Streetcodes.Select(s => s.Id).ToList();
                newPartner.Streetcodes.AddRange(await _repositoryWrapper
                    .StreetcodeRepository
                    .GetAllAsync(s => streetcodeIds.Contains(s.Id)));

                _repositoryWrapper.SaveChanges();
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
