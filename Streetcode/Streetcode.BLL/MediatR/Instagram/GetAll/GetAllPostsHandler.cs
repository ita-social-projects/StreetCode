using FluentResults;
using Streetcode.BLL.Interfaces.Instagram;
using Streetcode.BLL.MediatR.Payment;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.BLL.MediatR.Instagram.GetAll
{
    public class GetAllPostsHandler
    {
        private const int _limit = 10;
        private readonly IInstagramService _instagramService;
        public GetAllPostsHandler(IInstagramService instagramService)
        {
            _instagramService = instagramService;
        }

        public async Task<Result<InvoiceInfo>> Handle(GetAllPostsHandler request, CancellationToken cancellationToken)
        {
            var result = await _instagramService.GetPostsAsync();

            return Result.Ok(result);
        }
    }
}

/*public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, Result<InvoiceInfo>>
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
    }*/