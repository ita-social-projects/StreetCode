using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Transactions;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetById;

public record GetTransactLinkByIdQuery(int Id) : IRequest<Result<TransactLinkDTO>>;
