using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;

public class GetTextByStreetcodeIdHandler : IRequestHandler<GetTextByStreetcodeIdQuery, Result<TextDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTextByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<TextDTO>> Handle(GetTextByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var text = await _repositoryWrapper.TextRepository.GetFirstOrDefaultAsync(text => text.StreetcodeId == request.StreetcodeId);

        if (text is null)
        {
            return Result.Fail(new Error($"Cannot find any text by the streetcode id: {request.StreetcodeId}"));
        }

        var textDto = _mapper.Map<TextDTO>(text);
        return Result.Ok(textDto);
    }
}