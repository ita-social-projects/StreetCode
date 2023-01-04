using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.DTO.Transactions;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;

public record GetTransactLinkByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<TransactLinkDTO>>;