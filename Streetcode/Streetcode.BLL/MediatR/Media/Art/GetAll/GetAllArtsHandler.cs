using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Art.GetAll;

public class GetAllArtsHandler : IRequestHandler<GetAllArtsQuery, Result<IEnumerable<ArtDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetAllArtsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<ArtDTO>>> Handle(GetAllArtsQuery request, CancellationToken cancellationToken)
    {
        var arts = await _repositoryWrapper.ArtRepository.GetAllAsync();

        if (arts is null)
        {
            const string errorMsg = $"Cannot find any arts";
            _logger?.LogError("GetAllArtsQuery handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var artDtos = _mapper.Map<IEnumerable<ArtDTO>>(arts);
        _logger?.LogInformation($"GetAllArtsQuery handled successfully");
        _logger?.LogInformation($"Retrieved {artDtos.Count()} arts");
        return Result.Ok(artDtos);
    }
}