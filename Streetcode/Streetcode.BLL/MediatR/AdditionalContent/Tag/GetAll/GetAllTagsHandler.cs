using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, Result<IEnumerable<TagDTO>>>
{
    private readonly ILoggerService _loggerService;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllTagsHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService loggerService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _loggerService = loggerService;
    }

    public async Task<Result<IEnumerable<TagDTO>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        _loggerService.LogInformation("Entry into GetAllTagsHandler");

        var tags = await _repositoryWrapper.TagRepository.GetAllAsync();

        if (tags is null)
        {
            return Result.Fail(new Error($"Cannot find any tags"));
        }

        var tagDtos = _mapper.Map<IEnumerable<TagDTO>>(tags);
        return Result.Ok(tagDtos);
    }
}