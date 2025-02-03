using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;

public class GetTransactLinkByStreetcodeIdHandler : IRequestHandler<GetTransactLinkByStreetcodeIdQuery, Result<TransactLinkDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTransactLinkByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<TransactLinkDto>> Handle(GetTransactLinkByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var transactLink = await _repositoryWrapper.TransactLinksRepository
            .GetFirstOrDefaultAsync(f => f.StreetcodeId == request.StreetcodeId);

        if (transactLink is null)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository
                .GetFirstOrDefaultAsync(s => s.Id == request.StreetcodeId);
            if (streetcode == null)
            {
                string errorMsg = _stringLocalizerCannotFind["CannotFindTransactionLinkByStreetcodeIdBecause", request.StreetcodeId].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }

        NullResult<TransactLinkDto> result = new NullResult<TransactLinkDto>();
        result.WithValue(_mapper.Map<TransactLinkDto>(transactLink));
        return result;
    }
}