using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Helpers;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Services.EntityAccessManager;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, Result<GetAllTagsResponseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetAllTagsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> CannotFindSharedResource)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = CannotFindSharedResource;
    }

    public Task<Result<GetAllTagsResponseDTO>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.AdditionalContent.Tag, bool>>? basePredicate = null;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, t => t.Streetcodes);

        PaginationResponse<DAL.Entities.AdditionalContent.Tag> paginationResponse = _repositoryWrapper
            .TagRepository
            .GetAllPaginated(
                request.Page,
                request.PageSize,
                descendingSortKeySelector: tag => tag.Title,
                predicate: predicate);

        if (paginationResponse is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTags"].Value;
            _logger.LogError(request, errorMsg);
            return Task.FromResult(Result.Fail<GetAllTagsResponseDTO>(new Error(errorMsg)));
        }

        GetAllTagsResponseDTO getAllTagsResponseDTO = new GetAllTagsResponseDTO
        {
            TotalAmount = paginationResponse.TotalItems,
            Tags = _mapper.Map<IEnumerable<TagDTO>>(paginationResponse.Entities),
        };

        return Task.FromResult(Result.Ok(getAllTagsResponseDTO));
    }
}
