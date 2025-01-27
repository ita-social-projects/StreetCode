using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Event;
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

        public GetEventByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizer)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Result<EventDTO>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var eventDTO = _mapper.Map<EventDTO>(await _repositoryWrapper.EventRepository.GetFirstOrDefaultAsync(e => e.Id == request.id));

            if (eventDTO is null)
            {
                string errorMsg = _stringLocalizer["NoEventsByEnteredId", request.id].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            return Result.Ok(eventDTO);
        }
    }
}
