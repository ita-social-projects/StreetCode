using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetById;

public class GetTransactLinkByIdHandler : IRequestHandler<GetTransactLinkByIdQuery, Result<TransactLinkDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTransactLinkByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<TransactLinkDTO>> Handle(GetTransactLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var transactLink = await _repositoryWrapper.TransactLinksRepository
            .GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (transactLink is null)
        {
            return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindAnyTransactionLinkWithCorrespondingId", request.Id]));
        }

        var mappedTransactLink = _mapper.Map<TransactLinkDTO>(transactLink);
        return Result.Ok(mappedTransactLink);
    }
}