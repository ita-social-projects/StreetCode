using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete
{
    public class DeleteRelatedTermHandler : IRequestHandler<DeleteRelatedTermCommand, Result<RelatedTermDTO>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public DeleteRelatedTermHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<RelatedTermDTO>> Handle(DeleteRelatedTermCommand request, CancellationToken cancellationToken)
        {
            var relatedTerm = await _repository.RelatedTermRepository.GetFirstOrDefaultAsync(rt => rt.Word.ToLower().Equals(request.word.ToLower()));

            if (relatedTerm is null)
            {
                return Result.Fail(new Error($"Cannot find a related term: {request.word}"));
            }

            _repository.RelatedTermRepository.Delete(relatedTerm);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

            var relatedTermDto = _mapper.Map<RelatedTermDTO>(relatedTerm);

            return resultIsSuccess && relatedTermDto != null ? Result.Ok(relatedTermDto) : Result.Fail(new Error("Failed to delete a related term"));
        }
    }
}
