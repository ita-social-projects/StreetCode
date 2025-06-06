using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetAll;

public class GetAllTransactLinksHandler : IRequestHandler<GetAllTransactLinksQuery, Result<IEnumerable<TransactLinkDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetAllTransactLinksHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<TransactLinkDTO>>> Handle(GetAllTransactLinksQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.Transactions.TransactionLink, bool>>? basePredicate = null;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, t => t.Streetcode);

        var transactLinks = await _repositoryWrapper.TransactLinksRepository.GetAllAsync(predicate: predicate);

        if (transactLinks is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTransactionLink"].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<IEnumerable<TransactLinkDTO>>(transactLinks));
    }
}
