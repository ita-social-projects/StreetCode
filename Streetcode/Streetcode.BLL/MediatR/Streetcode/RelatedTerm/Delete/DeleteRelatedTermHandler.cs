using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete
{
    public class DeleteRelatedTermHandler : IRequestHandler<DeleteRelatedTermCommand, Result<DAL.Entities.Streetcode.TextContent.RelatedTerm>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public DeleteRelatedTermHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<DAL.Entities.Streetcode.TextContent.RelatedTerm>> Handle(DeleteRelatedTermCommand request, CancellationToken cancellationToken)
        {
            var relatedTerm = await _repository.RelatedTermRepository.GetFirstOrDefaultAsync(rt => rt.Word.ToLower().Equals(request.word.ToLower()));

            if (relatedTerm is null)
            {
                return Result.Fail(new Error($"Cannot find a related term: {request.word}"));
            }

            _repository.RelatedTermRepository.Delete(relatedTerm);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(relatedTerm) : Result.Fail(new Error("Failed to delete a related term"));
        }
    }
}
