using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Factories.Event;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Event.Update
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;
        private readonly IStringLocalizer<AnErrorOccurredSharedResource> _stringLocalizerAnErrorOccurred;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public UpdateEventHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate,
            IStringLocalizer<AnErrorOccurredSharedResource> stringLocalizerAnErrorOccurred,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
            _stringLocalizerAnErrorOccurred = stringLocalizerAnErrorOccurred;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<int>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            using(var transactionScope = _repositoryWrapper.BeginTransaction())
            {
                try
                {
                    var eventToUpdate = await _repositoryWrapper.EventRepository
                        .GetFirstOrDefaultAsync(
                            e => e.Id == request.Event.Id,
                            include: q => q.Include(e => e.EventStreetcodes));

                    if (eventToUpdate == null)
                    {
                        var exMessage = _stringLocalizerCannotFind["CannotFindEventWithCorrespondingId", request.Event.Id].Value;
                        _logger.LogError(request, exMessage);
                        return Result.Fail(exMessage);
                    }

                    bool eventTypeChanged = eventToUpdate.EventType != request.Event.EventType.ToString();

                    if(eventTypeChanged)
                    {
                        if (eventToUpdate is HistoricalEvent historicalEvent)
                        {
                            historicalEvent.TimelineItemId = null;
                        }
                        else if (eventToUpdate is CustomEvent customEvent)
                        {
                            customEvent.Location = null;
                            customEvent.Organizer = null;
                        }

                        eventToUpdate = EventFactory.CreateEvent(request.Event.EventType);
                        eventToUpdate.Id = request.Event.Id;
                    }

                    _mapper.Map(request.Event, eventToUpdate);

                    if (eventToUpdate is HistoricalEvent updatedHistoricalEvent)
                    {
                        updatedHistoricalEvent.TimelineItemId = request.Event.TimelineItemId;
                    }
                    else if (eventToUpdate is CustomEvent updatedCustomEvent)
                    {
                        updatedCustomEvent.Location = request.Event.Location;
                        updatedCustomEvent.Organizer = request.Event.Organizer;
                    }

                    eventToUpdate.EventStreetcodes?.Clear();

                    if (request.Event.StreetcodeIds != null)
                    {
                        var existingStreetcodeLinks = await _repositoryWrapper.EventStreetcodesRepository
                            .GetAllAsync(es => es.EventId == eventToUpdate.Id);

                        var existingStreetcodeIds = existingStreetcodeLinks.Select(es => es.StreetcodeId).ToList();

                        var streetcodesToAdd = request.Event.StreetcodeIds
                            .Where(id => !existingStreetcodeIds.Contains(id))
                            .Select(id => new EventStreetcodes { EventId = eventToUpdate.Id, StreetcodeId = id })
                            .ToList();

                        var streetcodesToRemove = existingStreetcodeLinks
                            .Where(es => !request.Event.StreetcodeIds.Contains(es.StreetcodeId))
                            .ToList();

                        if (streetcodesToAdd.Any())
                        {
                            await _repositoryWrapper.EventStreetcodesRepository.CreateRangeAsync(streetcodesToAdd);
                        }

                        if (streetcodesToRemove.Any())
                        {
                            _repositoryWrapper.EventStreetcodesRepository.DeleteRange(streetcodesToRemove);
                        }

                        await _repositoryWrapper.SaveChangesAsync();
                    }

                    _repositoryWrapper.EventRepository.Update(eventToUpdate);
                    var isResultSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

                    if (isResultSuccess)
                    {
                        transactionScope.Complete();
                        return Result.Ok(eventToUpdate.Id);
                    }
                    else
                    {
                        string errorMsg = _stringLocalizerFailedToUpdate["FailedToUpdateEvent"].Value;
                        _logger.LogError(request, errorMsg);
                        return Result.Fail(new Error(errorMsg));
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = _stringLocalizerAnErrorOccurred["AnErrorOccurredWhileUpdatingEvent", ex.Message].Value;

                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }
            }
        }
    }
}