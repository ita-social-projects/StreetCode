using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetAll
{
    public class GetAllTermsHandler : IRequestHandler<GetAllTermsQuery, Result<IEnumerable<TermDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetAllTermsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<TermDTO>>> Handle(GetAllTermsQuery request, CancellationToken cancellationToken)
        {
            var terms = await _repositoryWrapper.TermRepository.GetAllAsync();

            if (!terms.Any())
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTerm"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<TermDTO>>(terms));
        }
    }
}
