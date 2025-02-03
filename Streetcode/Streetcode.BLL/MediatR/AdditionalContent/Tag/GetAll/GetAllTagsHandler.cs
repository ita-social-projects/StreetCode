using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Helpers;
using Streetcode.BLL.DTO.AdditionalContent.Tag;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, Result<GetAllTagsResponseDto>>
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

    public Task<Result<GetAllTagsResponseDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        PaginationResponse<DAL.Entities.AdditionalContent.Tag> paginationResponse = _repositoryWrapper
            .TagRepository
            .GetAllPaginated(
                request.page,
                request.pageSize,
                descendingSortKeySelector: tag => tag.Title);

        if (paginationResponse is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTags"].Value;
            _logger.LogError(request, errorMsg);
            return Task.FromResult(Result.Fail<GetAllTagsResponseDto>(new Error(errorMsg)));
        }

        GetAllTagsResponseDto getAllTagsResponseDTO = new GetAllTagsResponseDto
        {
            TotalAmount = paginationResponse.TotalItems,
            Tags = _mapper.Map<IEnumerable<TagDto>>(paginationResponse.Entities),
        };

        return Task.FromResult(Result.Ok(getAllTagsResponseDTO));
    }
}
