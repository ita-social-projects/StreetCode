using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
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
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CreateInvoiceHandler> _stringlocalization;

        public CreateInvoiceHandler(IPaymentService paymentService, ILoggerService logger, IStringLocalizer<CreateInvoiceHandler> stringLocalizer)
        {
            _paymentService = paymentService;
            _logger = logger;
            _stringlocalization = stringLocalizer;
        }

        public async Task<Result<InvoiceInfo>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = new Invoice(request.Payment.Amount * _currencyMultiplier, _hryvnyaCurrencyCode, new MerchantPaymentInfo { Destination = _stringlocalization["VoluntaryContribution"].Value }, request.Payment.RedirectUrl);
            return Result.Ok(await _paymentService.CreateInvoiceAsync(invoice));
        }
    }
}
