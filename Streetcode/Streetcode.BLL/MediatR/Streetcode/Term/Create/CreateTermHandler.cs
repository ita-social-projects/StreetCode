using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Create
{
    public class CreateTermHandler : IRequestHandler<CreateTermCommand, Result<TermDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvert;
        private readonly IStringLocalizer<CannotCreateSharedResource> _stringLocalizerCannotCreate;
        private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailedToCreate;

        public CreateTermHandler(
            IMapper mapper,
            IRepositoryWrapper repository,
            ILoggerService logger,
            IStringLocalizer<CannotCreateSharedResource> stringLocalizerCannotCreate,
            IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailedToCreate,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvert)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
            _stringLocalizerCannotCreate = stringLocalizerCannotCreate;
            _stringLocalizerFailedToCreate = stringLocalizerFailedToCreate;
            _stringLocalizerCannotConvert = stringLocalizerCannotConvert;
        }

        public async Task<Result<TermDTO>> Handle(CreateTermCommand request, CancellationToken cancellationToken)
        {
            var term = _mapper.Map<DAL.Entities.Streetcode.TextContent.Term>(request.Term);

            if (term is null)
            {
                string errorMsg = _stringLocalizerCannotConvert["CannotConvertNullToTerm"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdTerm = await _repository.TermRepository.CreateAsync(term);

            if (createdTerm is null)
            {
                string errorMsg = _stringLocalizerCannotCreate["CannotCreateTerm"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

            if(!resultIsSuccess)
            {
                string errorMsg = _stringLocalizerFailedToCreate["FailedToCreateTerm"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdTermDTO = _mapper.Map<TermDTO>(createdTerm);

            if(createdTermDTO != null)
            {
                return Result.Ok(createdTermDTO);
            }
            else
            {
                string errorMsg = _stringLocalizerFailedToCreate["FailedToMapCreatedTerm"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
