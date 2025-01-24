using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Jobs;
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
                        request.pageSize);

            if (paginationResponse is null)
            {
                string errorMsg = _stringLocalizeCannotFind["CannotFindAnyEvents"].Value;
                _logger.LogError(request, errorMsg);
                return Task.FromResult(Result.Fail<GetAllEventsResponseDTO>(new Error(errorMsg)));
            }

            GetAllEventsResponseDTO getAllEventsResponseDTO = new GetAllEventsResponseDTO()
            {
                TotalAmount = paginationResponse.TotalItems,
                Events = _mapper.Map<IEnumerable<EventDTO>>(paginationResponse.Entities),
            };

            return Task.FromResult(Result.Ok(getAllEventsResponseDTO));
        }
    }
}
