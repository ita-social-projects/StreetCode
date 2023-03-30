using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll
{
    public class GetAllHistoricalContextHandler : IRequestHandler<GetAllHistoricalContextQuery, Result<IEnumerable<HistoricalContextDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllHistoricalContextHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<HistoricalContextDTO>>> Handle(GetAllHistoricalContextQuery request, CancellationToken cancellationToken)
        {
            var historicalContextItems = await _repositoryWrapper
                .HistoricalContextRepository
                .GetAllAsync();

            if (historicalContextItems is null)
            {
                return Result.Fail(new Error($"Cannot find any historical contexts"));
            }

            return Result.Ok(_mapper.Map<IEnumerable<HistoricalContextDTO>>(historicalContextItems));
        }
    }
}
