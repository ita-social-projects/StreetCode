using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetById;

public class GetTransactLinkByIdHandler : IRequestHandler<GetTransactLinkByIdQuery, Result<TransactLinkDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTransactLinkByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<TransactLinkDTO>> Handle(GetTransactLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var transactLink = await _repositoryWrapper.TransactLinksRepository
            .GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (transactLink is null)
        {
            return Result.Fail(new Error($"Cannot find any transaction link with corresponding id: {request.Id}"));
        }

        var mappedTransactLink = _mapper.Map<TransactLinkDTO>(transactLink);
        return Result.Ok(mappedTransactLink);
    }
}