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
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Event.GetAll
{
    public class GetAllEventsHandler : IRequestHandler<GetAllEventsQuery, Result<GetAllEventsResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizeCannotFind;

        public GetAllEventsHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<CannotFindSharedResource> stringLocalizeCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizeCannotFind = stringLocalizeCannotFind;
        }

        public Task<Result<GetAllEventsResponseDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            PaginationResponse<DAL.Entities.Event.Event> paginationResponse = _repositoryWrapper
                    .EventRepository
                    .GetAllPaginated(
                        request.page,
                        request.pageSize,
                        predicate: !string.IsNullOrEmpty(request.EventType)
                        ? e => e.EventType == request.EventType
                        : null,
                        include: query => query
                            .Include(e => e.EventStreetcodes !)
                            .ThenInclude(es => es.StreetcodeContent !));

            if (paginationResponse is null)
            {
                string errorMsg = _stringLocalizeCannotFind["CannotFindAnyEvents"].Value;
                _logger.LogError(request, errorMsg);
                return Task.FromResult(Result.Fail<GetAllEventsResponseDto>(new Error(errorMsg)));
            }

            IEnumerable<DAL.Entities.Event.Event> filteredEvents = paginationResponse.Entities;

            IEnumerable<object> mappedEvents = filteredEvents
               .Select(e =>
               {
                   var eventDto = e switch
                   {
                       HistoricalEvent historicalEvent => _mapper.Map<HistoricalEventDto>(historicalEvent),
                       CustomEvent customEvent => _mapper.Map<CustomEventDto>(customEvent),
                       _ => _mapper.Map<EventDto>(e)
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

            GetAllEventsResponseDto getAllEventsResponseDTO = new GetAllEventsResponseDto()
            {
                TotalAmount = paginationResponse.TotalItems,
                Events = mappedEvents,
            };

            return Task.FromResult(Result.Ok(getAllEventsResponseDTO));
        }
    }
}