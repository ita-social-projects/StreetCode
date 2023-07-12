using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetAll
{
    public class GetAllTermsHandler : IRequestHandler<GetAllTermsQuery, Result<IEnumerable<TermDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllTermsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TermDTO>>> Handle(GetAllTermsQuery request, CancellationToken cancellationToken)
        {
            var terms = await _repositoryWrapper.TermRepository.GetAllAsync();

            if (terms is null)
            {
                const string errorMsg = $"Cannot find any term";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<TermDTO>>(terms));
        }
    }
}
