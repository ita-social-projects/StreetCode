using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Event.GetAllShort;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Event.GetAll
{
    public class GetAllEventsShortHandler : IRequestHandler<GetAllEventsShortQuery, Result<IEnumerable<EventShortDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizeCannotFind;

        public GetAllEventsShortHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizeCannotFind)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizeCannotFind = stringLocalizeCannotFind;
        }

        public async Task<Result<IEnumerable<EventShortDto>>> Handle(GetAllEventsShortQuery request, CancellationToken cancellationToken)
        {
            var events = await _repositoryWrapper.EventRepository.GetAllAsync();

            if (events is null)
            {
                string errorMsg = _stringLocalizeCannotFind["CannotFindAnyEvents"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<EventShortDto>>(events));
        }
    }
}
