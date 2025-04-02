using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;

public class GetTransactLinkByStreetcodeIdHandler : IRequestHandler<GetTransactLinkByStreetcodeIdQuery, Result<TransactLinkDTO>>
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

    public async Task<Result<TransactLinkDTO>> Handle(GetTransactLinkByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<StreetcodeContent, bool>>? streetcodeBasePredicate = t => t.Id == request.StreetcodeId;
        var streetcodePredicate = streetcodeBasePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

        var isStreetcodeExists = await _repositoryWrapper.StreetcodeRepository.FindAll(predicate: streetcodePredicate).AnyAsync(cancellationToken);

        if (!isStreetcodeExists)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindTransactionLinkByStreetcodeIdBecause", request.StreetcodeId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var transactLink = await _repositoryWrapper.TransactLinksRepository
            .GetFirstOrDefaultAsync(f => f.StreetcodeId == request.StreetcodeId);

        if (transactLink is null)
        {
            var infoMessage = _stringLocalizerCannotFind["CannotFindAnyTransactionLink"].Value;
            _logger.LogInformation(infoMessage);
            return Result.Ok();
        }

        return Result.Ok(_mapper.Map<TransactLinkDTO>(transactLink));
    }
}