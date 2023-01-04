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
        var transactLinks = await _repositoryWrapper.TransactLinksRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (transactLinks is null)
        {
            return Result.Fail(new Error($"Cannot find a transactLinks with corresponding Id: {request.Id}"));
        }

        var transactLinksDto = _mapper.Map<TransactLinkDTO>(transactLinks);
        return Result.Ok(transactLinksDto);
    }
}