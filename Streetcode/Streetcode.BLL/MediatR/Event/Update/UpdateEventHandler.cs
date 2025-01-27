using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Factories.Event;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Enums;
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

        public UpdateEventHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate, IStringLocalizer<AnErrorOccurredSharedResource> stringLocalizerAnErrorOccurred)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
            _stringLocalizerAnErrorOccurred = stringLocalizerAnErrorOccurred;
        }

        public async Task<Result<int>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            using(var transactionScope = _repositoryWrapper.BeginTransaction())
            {
                try
                {
                    var eventToUpdate = await _repositoryWrapper.EventRepository
                        .GetFirstOrDefaultAsync(e => e.Id == request.Event.Id);

                    if (eventToUpdate == null)
                    {
                        return Result.Fail(new Error("Event not found"));
                    }

                    _mapper.Map(request.Event, eventToUpdate);

                    var discriminatorProperty = _repositoryWrapper.EventRepository.Entry(eventToUpdate).Property<string>(EventTypeDiscriminators.DiscriminatorName);
                    discriminatorProperty.CurrentValue = EventTypeDiscriminators.GetEventType(request.Event.EventType);
                    discriminatorProperty.IsModified = true;

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
                    string errorMsg = _stringLocalizerAnErrorOccurred["AnErrorOccurredWhileUpdating", ex.Message].Value;

                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }
            }
        }
    }
}
