using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Entities.Team;

namespace Streetcode.BLL.MediatR.Team.Position.GetAll
{
    public class GetAllPositionsHandler : IRequestHandler<GetAllPositionsQuery, Result<GetAllPositionsDto>>
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

        public Task<Result<GetAllPositionsDto>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
        {
            PaginationResponse<Positions> paginationResponse = _repositoryWrapper
                .PositionRepository
                .GetAllPaginated(
                    request.page,
                    request.pageSize,
                    ascendingSortKeySelector: position => position.Position!);

            if (paginationResponse is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyPositions"].Value;
                _logger.LogError(request, errorMsg);
                return Task.FromResult(Result.Fail<GetAllPositionsDto>(new Error(errorMsg)));
            }

            GetAllPositionsDto getAllPositionsDTO = new GetAllPositionsDto()
            {
                TotalAmount = paginationResponse.TotalItems,
                Positions = _mapper.Map<IEnumerable<PositionDto>>(paginationResponse.Entities),
            };

            return Task.FromResult(Result.Ok(getAllPositionsDTO));
        }
    }
}
