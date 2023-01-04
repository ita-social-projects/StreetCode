using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
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
        var transactLinks = await _repositoryWrapper.TransactLinksRepository
            .GetSingleOrDefaultAsync(f => f.Streetcode.Id == request.StreetcodeId);

        if (transactLinks is null)
        {
            return Result.Fail(new Error($"Cannot find a transactLinks by a streetcode Id: {request.StreetcodeId}"));
        }

        var transactLinksDto = _mapper.Map<TransactLinkDTO>(transactLinks);
        return Result.Ok(transactLinksDto);
    }
}