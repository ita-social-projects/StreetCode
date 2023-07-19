using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.Delete
{
    public class DeletePartnerHandler : IRequestHandler<DeletePartnerQuery, Result<PartnerDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public DeletePartnerHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<PartnerDTO>> Handle(DeletePartnerQuery request, CancellationToken cancellationToken)
        {
            var partner = await _repositoryWrapper.PartnersRepository.GetFirstOrDefaultAsync(p => p.Id == request.id);
            if (partner == null)
            {
                const string errorMsg = _stringLocalizerNo?["NoPartnerWithSuchId"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }
            else
            {
                _repositoryWrapper.PartnersRepository.Delete(partner);
                try
                {
                    _repositoryWrapper.SaveChanges();
                    return Result.Ok(_mapper.Map<PartnerDTO>(partner));
                }
                catch(Exception ex)
                {
                    _logger.LogError(request, ex.Message);
                    return Result.Fail(ex.Message);
                }
            }
        }
    }
}
