using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetAllByTermId
{
    public record GetAllRelatedTermsByTermIdHandler : IRequestHandler<GetAllRelatedTermsByTermIdQuery, Result<IEnumerable<RelatedTermDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly IStringLocalizer<CannotGetSharedResource> _stringLocalizerCannotGet;
        private readonly IStringLocalizer<CannotCreateSharedResource> _stringLocalizerCannotCreate;

        public GetAllRelatedTermsByTermIdHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            IStringLocalizer<CannotGetSharedResource> stringLocalizerCannotGet,
            IStringLocalizer<CannotCreateSharedResource> stringLocalizerCannotCreate)
        {
            _mapper = mapper;
            _repository = repositoryWrapper;
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
                return new Error(_stringLocalizerCannotGet["CannotGetWordsByTermId"].Value);
            }

            var relatedTermsDTO = _mapper.Map<IEnumerable<RelatedTermDTO>>(relatedTerms);

            if (relatedTermsDTO is null)
            {
                return new Error(_stringLocalizerCannotCreate["CannotCreateDTOsForRelatedWords"].Value);
            }

            return Result.Ok(relatedTermsDTO);
        }
    }
}
