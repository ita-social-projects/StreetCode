using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Payment;
using Streetcode.BLL.MediatR.Payment;

namespace Streetcode.WebApi.Controllers.Payment
{
    public class PaymentController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] PaymentDTO payment)
        {
            return HandleResult(await Mediator.Send(new CreateInvoiceCommand(payment)));
        }
    }
}
