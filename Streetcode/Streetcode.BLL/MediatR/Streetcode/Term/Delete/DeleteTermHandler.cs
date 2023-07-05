using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Delete
{
    public class DeleteTermHandler : IRequestHandler<DeleteTermCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;

        public DeleteTermHandler(IRepositoryWrapper repository, ILoggerService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteTermCommand request, CancellationToken cancellationToken)
        {
            var term = await _repository.TermRepository.GetFirstOrDefaultAsync((term) => term.Id == request.id);

            if (term is null)
            {
                const string errorMsg = "Cannot convert null to Term";
                _logger.LogError($"DeleteTermCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }

            _repository.TermRepository.Delete(term);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            if(resultIsSuccess)
            {
                return Result.Ok(Unit.Value);
            }
            else
            {
                const string errorMsg = "Failed to delete a term";
                _logger.LogError($"DeleteTermCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
