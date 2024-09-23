using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Entities.Team;

namespace Streetcode.BLL.MediatR.Team.Position.GetAll
{
    public class GetAllPositionsHandler : IRequestHandler<GetAllPositionsQuery, Result<GetAllPositionsDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetAllPositionsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public Task<Result<GetAllPositionsDTO>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
        {
            PaginationResponse<Positions> paginationResponse = _repositoryWrapper
                .PositionRepository
                .GetAllPaginated(
                    request.page,
                    request.pageSize);

            if (paginationResponse is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyPositions"].Value;
                _logger.LogError(request, errorMsg);
                return Task.FromResult(Result.Fail<GetAllPositionsDTO>(new Error(errorMsg)));
            }

            GetAllPositionsDTO getAllPositionsDTO = new GetAllPositionsDTO()
            {
                TotalAmount = paginationResponse.TotalItems,
                Positions = _mapper.Map<IEnumerable<PositionDTO>>(paginationResponse.Entities),
            };

            return Task.FromResult(Result.Ok(getAllPositionsDTO));
        }
    }
}
