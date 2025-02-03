using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Payment;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.BLL.MediatR.Payment;

public record CreateInvoiceCommand(PaymentDto Payment)
    : IRequest<Result<InvoiceInfo>>;
