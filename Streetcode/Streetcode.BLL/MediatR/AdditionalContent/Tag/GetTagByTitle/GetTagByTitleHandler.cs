using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetTagByTitle;

public class GetTagByTitleHandler : IRequestHandler<GetTagByTitleQuery, Result<TagDTO>>
{
    private readonly ILoggerService _loggerService;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTagByTitleHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService loggerService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _loggerService = loggerService;
    }

    public async Task<Result<TagDTO>> Handle(GetTagByTitleQuery request, CancellationToken cancellationToken)
    {
        _loggerService.LogInformation("Enter into GetTagByTitleHandler");

        var tag = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(f => f.Title == request.Title);

        if (tag is null)
        {
            return Result.Fail(new Error($"Cannot find any tag by the title: {request.Title}"));
        }

        var tagDto = _mapper.Map<TagDTO>(tag);
        return Result.Ok(tagDto);
    }
}