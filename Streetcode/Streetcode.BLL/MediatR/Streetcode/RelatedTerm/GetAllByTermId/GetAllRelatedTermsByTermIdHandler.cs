using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetAllByTermId;

public record GetAllRelatedTermsByTermIdHandler : IRequestHandler<GetAllRelatedTermsByTermIdQuery, Result<IEnumerable<RelatedTermDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotCreateSharedResource> _stringLocalizerCannotCreate;

    public GetAllRelatedTermsByTermIdHandler(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IStringLocalizer<CannotCreateSharedResource> stringLocalizerCannotCreate)
    {
        _mapper = mapper;
        _repository = repositoryWrapper;
        _logger = logger;
        _stringLocalizerCannotCreate = stringLocalizerCannotCreate;
    }

    public async Task<Result<IEnumerable<RelatedTermDTO>>> Handle(GetAllRelatedTermsByTermIdQuery request, CancellationToken cancellationToken)
    {
        var relatedTerms = await _repository.RelatedTermRepository.GetAllAsync(
            x => x.TermId == request.Id,
            x => x.Include(rt => rt.Term!));

        var relatedTermsDto = _mapper.Map<IEnumerable<RelatedTermDTO>>(relatedTerms);

        if (relatedTermsDto is null)
        {
            var errorMessage = _stringLocalizerCannotCreate["CannotCreateDTOsForRelatedWords"].Value;
            _logger.LogError(request, errorMessage);
            return new Error(errorMessage);
        }

        return Result.Ok(relatedTermsDto);
    }
}
