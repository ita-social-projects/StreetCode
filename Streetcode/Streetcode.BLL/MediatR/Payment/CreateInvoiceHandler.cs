using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Payment;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.BLL.MediatR.Payment
{
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, Result<InvoiceInfo>>
    {
        private const int _hryvnyaCurrencyCode = 980;
        private const int _currencyMultiplier = 100;
        private readonly IPaymentService _paymentService;
        private readonly IStringLocalizer<CreateInvoiceHandler> _stringlocalization;

        public CreateInvoiceHandler(IPaymentService paymentService, IStringLocalizer<CreateInvoiceHandler> stringLocalizer)
        {
            _paymentService = paymentService;
            _stringlocalization = stringLocalizer;
        }

        public async Task<Result<InvoiceInfo>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = new Invoice(request.Payment.Amount * _currencyMultiplier, _hryvnyaCurrencyCode, new MerchantPaymentInfo { Destination = _stringlocalization["VoluntaryContribution"].Value }, request.Payment.RedirectUrl);

            var result = await _paymentService.CreateInvoiceAsync(invoice);

            return Result.Ok(result);
        }
    }
}
