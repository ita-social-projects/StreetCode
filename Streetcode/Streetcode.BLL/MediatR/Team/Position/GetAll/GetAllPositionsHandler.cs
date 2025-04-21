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
    public class GetAllPositionsHandler : IRequestHandler<GetAllPositionsQuery, Result<GetAllPositionsDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetAllPositionsHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILoggerService logger,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public Task<Result<GetAllPositionsDTO>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
        {
            var allPositions = _repositoryWrapper
                .PositionRepository
                .FindAll()
                .ToList();

            var filteredPositions = string.IsNullOrWhiteSpace(request.title)
                ? allPositions
                : allPositions
                    .Where(p => p.Position.Contains(request.title, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            var page = request.page ?? 1;
            var pageSize = request.pageSize ?? 10;

            var paginatedPositions = filteredPositions
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var getAllPositionsDTO = new GetAllPositionsDTO
            {
                TotalAmount = filteredPositions.Count,
                Positions = _mapper.Map<IEnumerable<PositionDTO>>(paginatedPositions),
            };

            return Task.FromResult(Result.Ok(getAllPositionsDTO));
        }
    }
}
