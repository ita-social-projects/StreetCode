using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete
{
    public class DeleteRelatedTermHandler : IRequestHandler<DeleteRelatedTermCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public DeleteRelatedTermHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteRelatedTermCommand request, CancellationToken cancellationToken)
        {
            var relatedTerm = await _repository.RelatedTermRepository.GetFirstOrDefaultAsync(rt => rt.Id == request.id);

            if (relatedTerm is null)
            {
                string errorMsg = $"Cannot find a related term with corresponding id: {request.id}";
                _logger.LogError($"DeleteRelatedTermCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }

            _repository.RelatedTermRepository.Delete(relatedTerm);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            if(resultIsSuccess)
            {
                return Result.Ok(Unit.Value);
            }
            else
            {
                string errorMsg = "Failed to delete a related term";
                _logger.LogError($"DeleteRelatedTermCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
