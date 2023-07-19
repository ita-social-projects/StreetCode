using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetAllByTermId
{
    public record GetAllRelatedTermsByTermIdHandler : IRequestHandler<GetAllRelatedTermsByTermIdQuery, Result<IEnumerable<RelatedTermDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotGetSharedResource> _stringLocalizerCannotGet;
        private readonly IStringLocalizer<CannotCreateSharedResource> _stringLocalizerCannotCreate;

        public GetAllRelatedTermsByTermIdHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<CannotGetSharedResource> stringLocalizerCannotGet,
            IStringLocalizer<CannotCreateSharedResource> stringLocalizerCannotCreate)
        {
            _mapper = mapper;
            _repository = repositoryWrapper;
            _logger = logger;
            _stringLocalizerCannotCreate = stringLocalizerCannotCreate;
            _stringLocalizerCannotGet = stringLocalizerCannotGet;
        }

        public async Task<Result<IEnumerable<RelatedTermDTO>>> Handle(GetAllRelatedTermsByTermIdQuery request, CancellationToken cancellationToken)
        {
            var relatedTerms = await _repository.RelatedTermRepository
                .GetAllAsync(
                predicate: rt => rt.TermId == request.id,
                include: rt => rt.Include(rt => rt.Term));

            if (relatedTerms is null)
            {
                const string errorMsg = _stringLocalizerCannotGet["CannotGetWordsByTermId"].Value;
                _logger.LogError(request, errorMsg);
                return new Error(errorMsg);
            }

            var relatedTermsDTO = _mapper.Map<IEnumerable<RelatedTermDTO>>(relatedTerms);

            if (relatedTermsDTO is null)
            {
                const string errorMsg = _stringLocalizerCannotCreate["CannotCreateDTOsForRelatedWords"].Value;
                _logger.LogError(request, errorMsg);
                return new Error(errorMsg);
            }

            return Result.Ok(relatedTermsDTO);
        }
    }
}
