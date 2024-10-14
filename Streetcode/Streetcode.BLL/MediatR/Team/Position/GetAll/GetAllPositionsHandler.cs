using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.Position.GetAll
{
    public class GetAllPositionsHandler : IRequestHandler<GetAllPositionsQuery, Result<IEnumerable<PositionDTO>>>
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

        public async Task<Result<IEnumerable<PositionDTO>>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
        {
            var positions = await _repositoryWrapper
                .PositionRepository
                .GetAllAsync();

            if (positions is null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindAnyPositions"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok(_mapper.Map<IEnumerable<PositionDTO>>(positions));
        }
    }
}
