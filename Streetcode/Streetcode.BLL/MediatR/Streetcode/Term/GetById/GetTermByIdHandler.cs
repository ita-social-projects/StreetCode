using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetById;

public class GetTermByIdHandler : IRequestHandler<GetTermByIdQuery, Result<TermDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTermByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<TermDTO>> Handle(GetTermByIdQuery request, CancellationToken cancellationToken)
    {
        var term = await _repositoryWrapper.TermRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (term is null)
        {
            return Result.Fail(new Error($"Cannot find any term with corresponding id: {request.Id}"));
        }

        var termDto = _mapper.Map<TermDTO>(term);
        return Result.Ok(termDto);
    }
}