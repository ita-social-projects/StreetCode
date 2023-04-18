using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Payment;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Payment;
using Streetcode.DAL.Entities.Payment;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Payment
{
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, Result<InvoiceInfo>>
    {
        private const int _hryvnyaCurrencyCode = 980;
        private const int _currencyMultiplier = 100;
        private readonly IPaymentService _paymentService;

        public CreateInvoiceHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<Result<InvoiceInfo>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = new Invoice(request.Payment.Amount * _currencyMultiplier, _hryvnyaCurrencyCode, new MerchantPaymentInfo { Destination = "Добровільний внесок на статутну діяльність ГО «Історична Платформа»" }, request.Payment.RedirectUrl);

            var result = await _paymentService.CreateInvoiceAsync(invoice);

            return Result.Ok(result);
        }
    }
}
