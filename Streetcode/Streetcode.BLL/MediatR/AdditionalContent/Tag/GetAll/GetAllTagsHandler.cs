using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, Result<IEnumerable<TagDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService? _logger;

    public GetAllTagsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<TagDTO>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repositoryWrapper.TagRepository.GetAllAsync();

        if (tags is null)
        {
            const string errorMsg = $"Cannot find any tags";
            _logger?.LogError("GetAllTagsQuery handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var tagDtos = _mapper.Map<IEnumerable<TagDTO>>(tags);
        _logger?.LogInformation($"GetAllTagsQuery handled successfully");
        _logger?.LogInformation($"Retrieved {tagDtos.Count()} tags");
        return Result.Ok(tagDtos);
    }
}
