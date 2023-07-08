using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll
{
    public class GetAllHistoricalContextHandler : IRequestHandler<GetAllHistoricalContextQuery, Result<IEnumerable<HistoricalContextDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetAllHistoricalContextHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<IEnumerable<HistoricalContextDTO>>> Handle(GetAllHistoricalContextQuery request, CancellationToken cancellationToken)
        {
            var historicalContextItems = await _repositoryWrapper
                .HistoricalContextRepository
                .GetAllAsync();

            if (historicalContextItems is null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyHistoricalContexts"].Value));
            }

            return Result.Ok(_mapper.Map<IEnumerable<HistoricalContextDTO>>(historicalContextItems));
        }
    }
}
