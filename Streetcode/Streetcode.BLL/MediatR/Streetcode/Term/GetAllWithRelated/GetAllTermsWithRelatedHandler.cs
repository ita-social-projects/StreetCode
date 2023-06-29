using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetAllWithRelated
{
    public class GetAllTermsWithRelatedHandler : IRequestHandler<GetAllTermsWithRelatedQuery, Result<IEnumerable<TermDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        public GetAllTermsWithRelatedHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<Result<IEnumerable<TermDTO>>> Handle(GetAllTermsWithRelatedQuery request, CancellationToken cancellationToken)
        {
            
        }
    }
}
