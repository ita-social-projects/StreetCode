using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create
{
    public class CreateRelatedTermHandler : IRequestHandler<CreateRelatedTermCommand, Result<RelatedTermDTO>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CannotCreateSharedResource> _stringLocalizerCannotCreate;
        private readonly IStringLocalizer<CannotSaveSharedResource> _stringLocalizerCannotSave;
        private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;
        private readonly IStringLocalizer<CreateRelatedTermHandler> _stringLocalizer;

        public CreateRelatedTermHandler(
            IRepositoryWrapper repository,
            IMapper mapper,
            IStringLocalizer<CannotSaveSharedResource> stringLocalizerCannotSave,
            IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap,
            IStringLocalizer<CreateRelatedTermHandler> stringLocalizer,
            IStringLocalizer<CannotCreateSharedResource> stringLocalizerCannotCreate)
        {
            _repository = repository;
            _mapper = mapper;
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
                return Result.Fail(new Error(_stringLocalizerCannotCreate["CannotCreateNewRelatedWordForTerm"].Value));
            }

            var existingTerms = await _repository.RelatedTermRepository
                .GetAllAsync(
                predicate: rt => rt.TermId == request.RelatedTerm.TermId && rt.Word == request.RelatedTerm.Word);

            if (existingTerms is null || existingTerms.Any())
            {
                return Result.Fail(new Error(_stringLocalizer["WordWithThisDefinitionAlreadyExists"].Value));
            }

            var createdRelatedTerm = _repository.RelatedTermRepository.Create(relatedTerm);

            var isSuccessResult = await _repository.SaveChangesAsync() > 0;

            if(!isSuccessResult)
            {
                return Result.Fail(new Error(_stringLocalizerCannotSave["CannotSaveChangesInTheDatabaseAfterRelatedWordCreation"].Value));
            }

            var createdRelatedTermDTO = _mapper.Map<RelatedTermDTO>(createdRelatedTerm);

            return createdRelatedTermDTO != null ? Result.Ok(createdRelatedTermDTO) : Result.Fail(new Error(_stringLocalizerCannotMap["CannotMapEntity"].Value));
        }
    }
}
