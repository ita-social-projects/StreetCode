using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Text;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;

public class GetTextByStreetcodeIdHandler : IRequestHandler<GetTextByStreetcodeIdQuery, Result<TextDTO?>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ITextService _textService;
    private readonly ILoggerService _logger;

    public GetTextByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITextService textService, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _textService = textService;
        _logger = logger;
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
                string errorMsg = $"Cannot find a transaction link by a streetcode id: {request.StreetcodeId}, because such streetcode doesn`t exist";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }

        NullResult<TextDTO?> result = new NullResult<TextDTO?>();
        if (text != null)
        {
            text.TextContent = await _textService.AddTermsTag(text?.TextContent ?? "");
            result.WithValue(_mapper.Map<TextDTO?>(text));
        }

        return result;
    }
}