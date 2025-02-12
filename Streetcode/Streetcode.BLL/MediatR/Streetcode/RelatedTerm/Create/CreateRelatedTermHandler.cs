using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create
{
    public class CreateRelatedTermHandler : IRequestHandler<CreateRelatedTermCommand, Result<RelatedTermDTO>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotCreateSharedResource> _stringLocalizerCannotCreate;
        private readonly IStringLocalizer<CannotSaveSharedResource> _stringLocalizerCannotSave;
        private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;
        private readonly IStringLocalizer<CreateRelatedTermHandler> _stringLocalizer;

        public CreateRelatedTermHandler(
            IRepositoryWrapper repository,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<CannotSaveSharedResource> stringLocalizerCannotSave,
            IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap,
            IStringLocalizer<CreateRelatedTermHandler> stringLocalizer,
            IStringLocalizer<CannotCreateSharedResource> stringLocalizerCannotCreate)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotSave = stringLocalizerCannotSave;
            _stringLocalizerCannotMap = stringLocalizerCannotMap;
            _stringLocalizer = stringLocalizer;
            _stringLocalizerCannotCreate = stringLocalizerCannotCreate;
        }

        public async Task<Result<RelatedTermDTO>> Handle(CreateRelatedTermCommand request, CancellationToken cancellationToken)
        {
            var relatedTerm = _mapper.Map<Entity>(request.RelatedTerm);

            if (relatedTerm is null)
            {
                string errorMsg = _stringLocalizerCannotCreate["CannotCreateNewRelatedWordForTerm"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var existingTerms = await _repository.RelatedTermRepository
                .GetAllAsync(
                predicate: rt => rt.TermId == request.RelatedTerm.TermId && rt.Word == request.RelatedTerm.Word);

            if (existingTerms is null || existingTerms.Any())
            {
                string errorMsg = _stringLocalizer["WordWithThisDefinitionAlreadyExists"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdRelatedTerm = await _repository.RelatedTermRepository.CreateAsync(relatedTerm);

            var isSuccessResult = await _repository.SaveChangesAsync() > 0;

            if(!isSuccessResult)
            {
                string errorMsg = _stringLocalizerCannotSave["CannotSaveChangesInTheDatabaseAfterRelatedWordCreation"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdRelatedTermDTO = _mapper.Map<RelatedTermDTO>(createdRelatedTerm);

            if(createdRelatedTermDTO != null)
            {
                return Result.Ok(createdRelatedTermDTO);
            }
            else
            {
                string errorMsg = _stringLocalizerCannotMap["CannotMapEntity"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
