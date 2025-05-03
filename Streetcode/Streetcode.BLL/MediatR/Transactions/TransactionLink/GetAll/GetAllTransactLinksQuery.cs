using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Transactions.TransactionLink.GetAll;

public record GetAllTransactLinksQuery(UserRole? UserRole) : IRequest<Result<IEnumerable<TransactLinkDTO>>>;