using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
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

        public CreateInvoiceHandler(IPaymentService paymentService, ILoggerService logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<Result<InvoiceInfo>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = new Invoice(request.Payment.Amount * _currencyMultiplier, _hryvnyaCurrencyCode, new MerchantPaymentInfo { Destination = "Добровільний внесок на статутну діяльність ГО «Історична Платформа»" }, request.Payment.RedirectUrl);
            return Result.Ok(await _paymentService.CreateInvoiceAsync(invoice));
        }
    }
}
