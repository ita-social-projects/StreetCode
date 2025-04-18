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

public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, Result<GetAllTagsResponseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetAllTagsHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public Task<Result<GetAllTagsResponseDTO>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var allTags = _repositoryWrapper
            .TagRepository
            .FindAll()
            .ToList();

        if (allTags == null || !allTags.Any())
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTags"].Value;
            _logger.LogError(request, errorMsg);
            return Task.FromResult(Result.Fail<GetAllTagsResponseDTO>(new Error(errorMsg)));
        }

        var filteredTags = string.IsNullOrWhiteSpace(request.title)
            ? allTags
            : allTags.Where(t => t.Title.ToLower().Contains(request.title.ToLower()));

        var page = request.page ?? 1;
        var pageSize = request.pageSize ?? 10;

        var paginatedTags = filteredTags
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var getAllTagsResponseDTO = new GetAllTagsResponseDTO
        {
            TotalAmount = filteredTags.Count(),
            Tags = _mapper.Map<IEnumerable<TagDTO>>(paginatedTags),
        };

        return Task.FromResult(Result.Ok(getAllTagsResponseDTO));
    }
}
