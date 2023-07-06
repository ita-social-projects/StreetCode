using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetById;

public class GetTextByIdHandler : IRequestHandler<GetTextByIdQuery, Result<TextDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTextByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<TextDTO>> Handle(GetTextByIdQuery request, CancellationToken cancellationToken)
    {
        var text = await _repositoryWrapper.TextRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (text is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyTextWithCorrespondingId", request.Id].Value));
        }

        var textDto = _mapper.Map<TextDTO>(text);
        return Result.Ok(textDto);
    }
}