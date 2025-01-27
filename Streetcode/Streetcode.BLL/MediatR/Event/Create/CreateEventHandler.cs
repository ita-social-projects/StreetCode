using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Factories.Event;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

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
                    _repositoryWrapper.EventRepository.Create(eventEntity);
                    var isResultSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

                    if(isResultSuccess)
                    {
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
                    string errorMsg = _stringLocalizerAnErrorOccurred["AnErrorOccurredWhileCreating", ex.Message].Value;
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }
            }
        }
    }
}
