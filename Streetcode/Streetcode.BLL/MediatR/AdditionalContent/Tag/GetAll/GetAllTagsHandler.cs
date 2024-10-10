using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, Result<IEnumerable<TagDTO>>>
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

    public async Task<Result<IEnumerable<TagDTO>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repositoryWrapper.TagRepository.GetAllAsync();
        if (tags is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTags"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var sorted_tags = tags.OrderBy(tag => tag.Title);
        return Result.Ok(_mapper.Map<IEnumerable<TagDTO>>(sorted_tags));
    }
}
