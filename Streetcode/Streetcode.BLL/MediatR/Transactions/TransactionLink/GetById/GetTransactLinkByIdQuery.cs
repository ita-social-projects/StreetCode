using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetById;

public record GetTransactLinkByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<TransactLinkDTO>>;
