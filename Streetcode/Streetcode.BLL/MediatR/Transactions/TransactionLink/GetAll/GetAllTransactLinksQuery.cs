using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Transactions;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetAll;

public record GetAllTransactLinksQuery : IRequest<Result<IEnumerable<TransactLinkDTO>>>;