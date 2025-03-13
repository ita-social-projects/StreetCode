using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Event.GetById
{
    public class GetEventByIdHandler : IRequestHandler<GetEventByIdQuery, Result<EventDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizer;

        public GetEventByIdHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<CannotFindSharedResource> stringLocalizer)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Result<EventDTO>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var eventEntity = await _repositoryWrapper.EventRepository
                .GetFirstOrDefaultAsync(
                e => e.Id == request.id,
                include: e => e.Include(ev => ev.EventStreetcodes)
                      .ThenInclude(es => es.StreetcodeContent!));

            if (eventEntity is null)
            {
                string errorMsg = _stringLocalizer["CannotFindEventWithCorrespondingId", request.id].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            EventDTO mappedEvent;

            switch (eventEntity.EventType)
            {
                case "Historical":
                    mappedEvent = _mapper.Map<HistoricalEventDTO>(eventEntity);
                    break;
                case "Custom":
                    mappedEvent = _mapper.Map<CustomEventDTO>(eventEntity);
                    break;
                default:
                    mappedEvent = _mapper.Map<EventDTO>(eventEntity);
                    break;
            }

            if (eventEntity.EventStreetcodes != null)
            {
                mappedEvent.Streetcodes = eventEntity.EventStreetcodes
                    .Where(es => es.StreetcodeContent != null)
                    .Select(es => new StreetcodeShortDTO
                    {
                        Id = es.StreetcodeContent.Id,
                        Title = es.StreetcodeContent.Title
                    })
                    .ToList();
            }
            else
            {
                mappedEvent.Streetcodes = new List<StreetcodeShortDTO>();
            }

            return Result.Ok(mappedEvent);
        }
    }
}