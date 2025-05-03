using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;

public record GetTransactLinkByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
    : IRequest<Result<TransactLinkDTO>>;