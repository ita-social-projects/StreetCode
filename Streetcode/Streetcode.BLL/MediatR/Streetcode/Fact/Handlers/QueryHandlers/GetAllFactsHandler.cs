using AutoMapper;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.Queries;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Handlers.QueryHandlers
{
    public class GetAllFactsHandler : IRequestHandler<GetAllFactsQuery, IEnumerable<FactDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetAllFactsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FactDTO>> Handle(GetAllFactsQuery request, CancellationToken cancellationToken)
        {
            var facts = await _repositoryWrapper.FactRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<FactDTO>>(facts);
        }
    }
}
