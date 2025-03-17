using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetAll;

public class GetAllTermsHandler : IRequestHandler<GetAllTermsQuery, Result<GetAllTermsDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllTermsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public Task<Result<GetAllTermsDto>> Handle(GetAllTermsQuery request, CancellationToken cancellationToken)
    {
        var paginatedTerms = _repositoryWrapper.TermRepository.GetAllPaginated(request.page, request.pageSize);
        var getAllTermsDto = new GetAllTermsDto()
        {
            TotalAmount = paginatedTerms.TotalItems,
            Terms = _mapper.Map<IEnumerable<TermDto>>(paginatedTerms.Entities),
        };

        return Task.FromResult(Result.Ok(getAllTermsDto));
    }
}
