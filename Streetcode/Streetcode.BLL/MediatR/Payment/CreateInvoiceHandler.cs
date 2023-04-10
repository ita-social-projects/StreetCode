using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Payment;
using Streetcode.BLL.Interfaces.Payment;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.BLL.MediatR.Payment
{
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, Result<InvoiceInfo>>
    {
        private readonly IPaymentService _paymentService;
        public CreateInvoiceHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<Result<InvoiceInfo>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = new Invoice
            {
                Amount = request.Payment.Amount
            };
            var invoiceInfo = await _paymentService.CreateInvoiceAsync(invoice);
            return Result.Ok(invoiceInfo);
        }
    }
}
