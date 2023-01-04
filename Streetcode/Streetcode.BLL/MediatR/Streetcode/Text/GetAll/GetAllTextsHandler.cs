using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetAll;

public class GetAllTextsHandler : IRequestHandler<GetAllTextsQuery, Result<IEnumerable<TextDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllTextsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TextDTO>>> Handle(GetAllTextsQuery request, CancellationToken cancellationToken)
    {
        var texts = await _repositoryWrapper.TextRepository.GetAllAsync();

        var textDtos = _mapper.Map<IEnumerable<TextDTO>>(texts);
        return Result.Ok(textDtos);
    }
}