using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete
{
    public class DeleteRelatedTermHandler : IRequestHandler<DeleteRelatedTermCommand, Result<RelatedTermDTO>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

        public DeleteRelatedTermHandler(
            IRepositoryWrapper repository,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<RelatedTermDTO>> Handle(DeleteRelatedTermCommand request, CancellationToken cancellationToken)
        {
            var relatedTerm = await _repository.RelatedTermRepository.GetFirstOrDefaultAsync(rt => rt.Word!.ToLower().Equals(request.word.ToLower()));

            if (relatedTerm is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindRelatedTermWithCorrespondingId", request.word].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _repository.RelatedTermRepository.Delete(relatedTerm);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            var relatedTermDto = _mapper.Map<RelatedTermDTO>(relatedTerm);
            if(resultIsSuccess && relatedTermDto != null)
            {
                return Result.Ok(relatedTermDto);
            }
            else
            {
                string errorMsg = _stringLocalizerFailedToDelete["FailedToDeleteRelatedTerm"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
