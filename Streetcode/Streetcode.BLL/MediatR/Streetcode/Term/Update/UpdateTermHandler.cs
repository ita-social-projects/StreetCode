using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Update
{
    public class UpdateTermHandler : IRequestHandler<UpdateTermCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;

        public UpdateTermHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(UpdateTermCommand request, CancellationToken cancellationToken)
        {
            var term = _mapper.Map<DAL.Entities.Streetcode.TextContent.Term>(request.Term);
            if (term is null)
            {
                const string errorMsg = "Cannot convert null to Term";
                _logger.LogError($"UpdateTermCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }

            _repository.TermRepository.Update(term);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            if (resultIsSuccess)
            {
                return Result.Ok(Unit.Value);
            }
            else
            {
                const string errorMsg = "Failed to update a term";
                _logger.LogError($"UpdateTermCommand handled with an error. {errorMsg}");
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
