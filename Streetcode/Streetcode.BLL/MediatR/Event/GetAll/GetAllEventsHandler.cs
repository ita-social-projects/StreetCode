using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Event.GetAll
{
    public class GetAllEventsHandler : IRequestHandler<GetAllEventsQuery, Result<GetAllEventsResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizeCannotFind;

        public GetAllEventsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizeCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizeCannotFind = stringLocalizeCannotFind;
        }

        public Task<Result<GetAllEventsResponseDTO>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            PaginationResponse<DAL.Entities.Event.Event> paginationResponse = _repositoryWrapper
                    .EventRepository
                    .GetAllPaginated(
                        request.page,
                        request.pageSize,
                        include: query => query
                            .Include(e => e.EventStreetcodes)
                            .ThenInclude(es => es.StreetcodeContent));

            if (paginationResponse is null)
            {
                string errorMsg = _stringLocalizeCannotFind["CannotFindAnyEvents"].Value;
                _logger.LogError(request, errorMsg);
                return Task.FromResult(Result.Fail<GetAllEventsResponseDTO>(new Error(errorMsg)));
            }

            IEnumerable<DAL.Entities.Event.Event> filteredEvents = paginationResponse.Entities;

            if (request.EventType.HasValue)
            {
                string eventTypeString = request.EventType.ToString();
                filteredEvents = filteredEvents.Where(e => e.EventType == eventTypeString);
            }

            IEnumerable<object> mappedEvents = filteredEvents
               .Select(e =>
               {
                   var eventDto = e switch
                   {
                       HistoricalEvent historicalEvent => _mapper.Map<HistoricalEventDTO>(historicalEvent),
                       CustomEvent customEvent => _mapper.Map<CustomEventDTO>(customEvent),
                       _ => _mapper.Map<EventDTO>(e)
                   };

                   if(e.EventStreetcodes != null)
                   {
                       eventDto.Streetcodes = e.EventStreetcodes
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
                       eventDto.Streetcodes = new List<StreetcodeShortDTO>();
                   }

                   return eventDto;
               });

            GetAllEventsResponseDTO getAllEventsResponseDTO = new GetAllEventsResponseDTO()
            {
                TotalAmount = mappedEvents.Count(),
                Events = mappedEvents,
            };

            return Task.FromResult(Result.Ok(getAllEventsResponseDTO));
        }
    }
}
