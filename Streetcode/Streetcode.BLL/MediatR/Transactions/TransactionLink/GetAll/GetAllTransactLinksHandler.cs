using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetAll;

public class GetAllTransactLinksHandler : IRequestHandler<GetAllTransactLinksQuery, Result<IEnumerable<TransactLinkDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService? _logger;

    public GetAllTransactLinksHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService? logger = null)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<TransactLinkDTO>>> Handle(GetAllTransactLinksQuery request, CancellationToken cancellationToken)
    {
        var transactLinks = await _repositoryWrapper.TransactLinksRepository.GetAllAsync();

        if (transactLinks is null)
        {
            const string errorMsg = $"Cannot find any transaction link";
            _logger?.LogError("GetAllTransactLinksQuery handled with an error");
            _logger?.LogError(errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var transactLinksDtos = _mapper.Map<IEnumerable<TransactLinkDTO>>(transactLinks);
        _logger?.LogInformation($"GetAllTransactLinksQuery handled successfully");
        _logger?.LogInformation($"Retrieved {transactLinksDtos.Count()} transactLinks");
        return Result.Ok(transactLinksDtos);
    }
}
