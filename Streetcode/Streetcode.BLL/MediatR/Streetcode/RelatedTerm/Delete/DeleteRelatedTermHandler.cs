using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete
{
    public class DeleteRelatedTermHandler : IRequestHandler<DeleteRelatedTermCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;

        public DeleteRelatedTermHandler(
            IRepositoryWrapper repository,
            IMapper mapper,
            IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repository = repository;
            _mapper = mapper;
            _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<Unit>> Handle(DeleteRelatedTermCommand request, CancellationToken cancellationToken)
        {
            var relatedTerm = await _repository.RelatedTermRepository.GetFirstOrDefaultAsync(rt => rt.Id == request.id);

            if (relatedTerm is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindRelatedTermWithCorrespondingId", request.id].Value));
            }

            _repository.RelatedTermRepository.Delete(relatedTerm);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToDelete["FailedToDeleteRelatedTerm"].Value));
        }
    }
}
