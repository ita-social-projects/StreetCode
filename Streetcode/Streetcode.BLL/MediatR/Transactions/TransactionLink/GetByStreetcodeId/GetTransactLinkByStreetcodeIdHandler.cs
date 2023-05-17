using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;

public class GetTransactLinkByStreetcodeIdHandler : IRequestHandler<GetTransactLinkByStreetcodeIdQuery, Result<TransactLinkDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTransactLinkByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<TransactLinkDTO>> Handle(GetTransactLinkByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var transactLink = await _repositoryWrapper.TransactLinksRepository
            .GetFirstOrDefaultAsync(f => f.StreetcodeId == request.StreetcodeId);

        if (transactLink is null)
        {
            if (await _repositoryWrapper.StreetcodeRepository
                .GetFirstOrDefaultAsync(s => s.Id == request.StreetcodeId) == null)
            {
                return Result.Fail(new Error($"Cannot find a transaction link by a streetcode id: {request.StreetcodeId}, because such streetcode doesn`t exist"));
            }
            else
            {
                return Result.Ok();
            }
        }

        var mappedTransactLink = _mapper.Map<TransactLinkDTO>(transactLink);
        return Result.Ok(mappedTransactLink);
    }
}