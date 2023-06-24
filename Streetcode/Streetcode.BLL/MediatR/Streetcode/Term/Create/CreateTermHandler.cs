using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Create
{
    public class CreateTermHandler : IRequestHandler<CreateTermCommand, Result<TermDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService? _logger;

        public CreateTermHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService? logger = null)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<TermDTO>> Handle(CreateTermCommand request, CancellationToken cancellationToken)
        {
            var term = _mapper.Map<DAL.Entities.Streetcode.TextContent.Term>(request.Term);

            if (term is null)
            {
                const string errorMsg = "Cannot convert null to Term";
                _logger?.LogError("CreateTermCommand handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdTerm = _repository.TermRepository.Create(term);

            if (createdTerm is null)
            {
                const string errorMsg = "Cannot create term";
                _logger?.LogError("CreateTermCommand handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

            if(!resultIsSuccess)
            {
                const string errorMsg = "Failed to create a term";
                _logger?.LogError("CreateTermCommand handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var createdTermDTO = _mapper.Map<TermDTO>(createdTerm);

            if(createdTermDTO != null)
            {
                _logger?.LogInformation($"CreateTermCommand handled successfully");
                return Result.Ok(createdTermDTO);
            }
            else
            {
                const string errorMsg = "Failed to map created term";
                _logger?.LogError("CreateTermCommand handled with an error");
                _logger?.LogError(errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
