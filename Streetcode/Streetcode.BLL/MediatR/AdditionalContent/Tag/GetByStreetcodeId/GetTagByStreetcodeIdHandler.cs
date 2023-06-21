using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;

public class GetTagByStreetcodeIdHandler : IRequestHandler<GetTagByStreetcodeIdQuery, Result<IEnumerable<StreetcodeTagDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService? _logger;

    public GetTagByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<StreetcodeTagDTO>>> Handle(GetTagByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var tagIndexed = await _repositoryWrapper.StreetcodeTagIndexRepository
            .GetAllAsync(
                t => t.StreetcodeId == request.StreetcodeId,
                include: q => q.Include(t => t.Tag));

        if (tagIndexed is null)
        {
            string errorMsg = $"Cannot find any tag by the streetcode id: {request.StreetcodeId}";
            _logger?.LogError("GetTagByStreetcodeIdQuery handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var tagDto = _mapper.Map<IEnumerable<StreetcodeTagDTO>>(tagIndexed.OrderBy(ti => ti.Index));
        _logger?.LogInformation($"GetTagByStreetcodeIdQuery handled successfully");
        _logger?.LogInformation($"Retrieved {tagDto.Count()} tags");
        return Result.Ok(tagDto);
    }
}
