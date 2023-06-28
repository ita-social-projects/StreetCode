using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.Delete
{
    public class DeletePartnerHandler : IRequestHandler<DeletePartnerQuery, Result<PartnerDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public DeletePartnerHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PartnerDTO>> Handle(DeletePartnerQuery request, CancellationToken cancellationToken)
        {
            var partner = await _repositoryWrapper.PartnersRepository.GetFirstOrDefaultAsync(p => p.Id == request.id);
            if (partner == null)
            {
                const string errorMsg = "No partner with such id";
                _logger?.LogError("DeletePartnerQuery handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(errorMsg);
            }
            else
            {
                _repositoryWrapper.PartnersRepository.Delete(partner);
                try
                {
                    _repositoryWrapper.SaveChanges();
                    _logger?.LogInformation($"DeletePartnerQuery handled successfully");
                    return Result.Ok(_mapper.Map<PartnerDTO>(partner));
                }
                catch(Exception ex)
                {
                    _logger?.LogError("DeletePartnerQuery handled with an error");
                    _logger?.LogError(ex.Message);
                    return Result.Fail(ex.Message);
                }
            }
        }
    }
}
