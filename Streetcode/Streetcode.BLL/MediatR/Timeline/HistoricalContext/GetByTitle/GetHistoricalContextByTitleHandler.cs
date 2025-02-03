using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetByTitle;

public class GetHistoricalContextByTitleHandler : IRequestHandler<GetHistoricalContextByTitleQuery, Result<HistoricalContextDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _loggerService;
    private readonly IStringLocalizer<CannotFindSharedResource> _localizer;

    public GetHistoricalContextByTitleHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService loggerService, IStringLocalizer<CannotFindSharedResource> localizer)
    {
        _mapper = mapper;
        _repository = repository;
        _loggerService = loggerService;
        _localizer = localizer;
    }

    public async Task<Result<HistoricalContextDto>> Handle(GetHistoricalContextByTitleQuery request, CancellationToken cancellationToken)
    {
        var context = await _repository.HistoricalContextRepository.GetFirstOrDefaultAsync(j => j.Title == request.title);

        if (context is null)
        {
            string exceptionMessege = _localizer["CannotFindHistoricalContextWithTitle", request.title];
            _loggerService.LogError(request, exceptionMessege);
            return Result.Fail(exceptionMessege);
        }

        try
        {
            var contextDto = _mapper.Map<HistoricalContextDto>(context);
            return Result.Ok(contextDto);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}