using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetAll;

public class GetAllTermsHandler : IRequestHandler<GetAllTermsQuery, Result<IEnumerable<TermDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllTermsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TermDTO>>> Handle(GetAllTermsQuery request, CancellationToken cancellationToken)
    {
        var terms = await _repositoryWrapper.TermRepository.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(request.title))
        {
            terms = terms.Where(t => t.Title != null && t.Title.Contains(request.title, StringComparison.OrdinalIgnoreCase));
        }

        var termDTOs = _mapper.Map<IEnumerable<TermDTO>>(terms);
        return Result.Ok(termDTOs);
    }
}
