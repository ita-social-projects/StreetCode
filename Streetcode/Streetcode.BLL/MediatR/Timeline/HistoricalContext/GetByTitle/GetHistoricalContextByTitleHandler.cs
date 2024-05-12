using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetById;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetByTitle;

public class GetHistoricalContextByTitleHandler : IRequestHandler<GetHistoricalContextByTitleQuery, Result<HistoricalContextDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _loggerService;

    public GetHistoricalContextByTitleHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService loggerService)
    {
        _mapper = mapper;
        _repository = repository;
        _loggerService = loggerService;
    }

    public async Task<Result<HistoricalContextDTO>> Handle(GetHistoricalContextByTitleQuery request, CancellationToken cancellationToken)
    {
        var context = await _repository.HistoricalContextRepository.GetFirstOrDefaultAsync(j => j.Title == request.title);

        if (context is null)
        {
            string exceptionMessege = $"No context found by title - {request.title}";
            _loggerService.LogError(request, exceptionMessege);
            return Result.Fail(exceptionMessege);
        }

        try
        {
            var contextDto = _mapper.Map<HistoricalContextDTO>(context);
            return Result.Ok(contextDto);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}