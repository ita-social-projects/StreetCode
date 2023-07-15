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
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetTagByTitleHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<TagDTO>> Handle(GetTagByTitleQuery request, CancellationToken cancellationToken)
    {
        var tag = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(f => f.Title == request.Title);

        if (tag is null)
        {
            string errorMsg = $"Cannot find any tag by the title: {request.Title}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<TagDTO>(tag));
    }
}
