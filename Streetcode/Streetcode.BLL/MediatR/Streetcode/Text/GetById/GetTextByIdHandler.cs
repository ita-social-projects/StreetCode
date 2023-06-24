using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetById;

public class GetTextByIdHandler : IRequestHandler<GetTextByIdQuery, Result<TextDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService? _logger;

    public GetTextByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<TextDTO>> Handle(GetTextByIdQuery request, CancellationToken cancellationToken)
    {
        var text = await _repositoryWrapper.TextRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (text is null)
        {
            string errorMsg = $"Cannot find any text with corresponding id: {request.Id}";
            _logger?.LogError("GetAllSubtitlesQuery handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var textDto = _mapper.Map<TextDTO>(text);
        _logger?.LogInformation($"GetTextByIdQuery handled successfully");
        return Result.Ok(textDto);
    }
}