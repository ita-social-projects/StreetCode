using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetById;

public class GetTextByIdHandler : IRequestHandler<GetTextByIdQuery, Result<TextDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTextByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<TextDTO>> Handle(GetTextByIdQuery request, CancellationToken cancellationToken)
    {
        var text = await _repositoryWrapper.TextRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (text is null)
        {
            return Result.Fail(new Error($"Cannot find a text with corresponding categoryId: {request.Id}"));
        }

        var textDto = _mapper.Map<TextDTO>(text);
        return Result.Ok(textDto);
    }
}