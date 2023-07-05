using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;

public class GetTextByStreetcodeIdHandler : IRequestHandler<GetTextByStreetcodeIdQuery, Result<TextDTO?>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTextByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<TextDTO?>> Handle(GetTextByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var text = await _repositoryWrapper.TextRepository
            .GetFirstOrDefaultAsync(text => text.StreetcodeId == request.StreetcodeId);

        if (text is null)
        {
            if (await _repositoryWrapper.StreetcodeRepository
                 .GetFirstOrDefaultAsync(s => s.Id == request.StreetcodeId) == null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindTransactionLinkByStreetcodeIdBecause", request.StreetcodeId]));
            }
        }

        var textDto = _mapper.Map<TextDTO?>(text);
        NullResult<TextDTO?> result = new NullResult<TextDTO?>();
        result.WithValue(_mapper.Map<TextDTO?>(text));
        return result;
    }
}