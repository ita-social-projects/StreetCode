using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Factories.Event;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Event.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Repositories.Interfaces.Base;
using EventEntity = Streetcode.DAL.Entities.Event.Event;

namespace Streetcode.BLL.MediatR.Event.Create
{
    public class CreateEventHandler : IRequestHandler<CreateEventCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailedToCreate;
        private readonly IStringLocalizer<AnErrorOccurredSharedResource> _stringLocalizerAnErrorOccurred;
        public CreateEventHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailedToCreate,
            IStringLocalizer<AnErrorOccurredSharedResource> stringLocalizerAnErrorOccurred)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerFailedToCreate = stringLocalizerFailedToCreate;
            _stringLocalizerAnErrorOccurred = stringLocalizerAnErrorOccurred;
        }

        public async Task<Result<int>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            using(var transactionScope = _repositoryWrapper.BeginTransaction())
            {
                try
                {
                    var eventEntity = EventFactory.CreateEvent(request.Event.EventType);
                    _mapper.Map(request.Event, eventEntity);

                    SetSpecialEventFields(eventEntity, request);

                    _repositoryWrapper.EventRepository.Create(eventEntity);
                    var isResultSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

                    if (isResultSuccess)
                    {
                        await SetEventStreetcodes(eventEntity, request);

                        transactionScope.Complete();
                        return Result.Ok(eventEntity.Id);
                    }
                    else
                    {
                        string errorMsg = _stringLocalizerFailedToCreate["FailedToCreateEvent"].Value;
                        _logger.LogError(request, errorMsg);
                        return Result.Fail(new Error(errorMsg));
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = _stringLocalizerAnErrorOccurred["AnErrorOccurredWhileCreatingEvent", ex.Message].Value;
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }
            }
        }

        private void SetSpecialEventFields(EventEntity eventToCreate, CreateEventCommand request)
        {
            _mapper.Map(request.Event, eventToCreate);

            if (eventToCreate is HistoricalEvent historicalEvent)
            {
                historicalEvent.TimelineItemId = request.Event.TimelineItemId;
            }
            else if (eventToCreate is CustomEvent customEvent)
            {
                customEvent.DateString = request.Event.DateString;
                customEvent.Location = request.Event.Location;
                customEvent.Organizer = request.Event.Organizer;
            }
        }

        private async Task SetEventStreetcodes(EventEntity eventEntity, CreateEventCommand request)
        {
            if (request.Event.StreetcodeIds == null)
            {
                return;
            }

            var eventStreetcodes = request.Event.StreetcodeIds
                                .Select(id => new EventStreetcodes { EventId = eventEntity.Id, StreetcodeId = id })
                                .ToList();

            await _repositoryWrapper.EventStreetcodesRepository.CreateRangeAsync(eventStreetcodes);
            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}