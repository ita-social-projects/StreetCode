using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Payment;
using Streetcode.BLL.MediatR.Payment;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.WebApi.Controllers.Payment
{
    public class PaymentController : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(InvoiceInfo))]
        public async Task<IActionResult> CreateInvoice([FromBody] PaymentDTO payment)
        {
            return HandleResult(await Mediator.Send(new CreateInvoiceCommand(payment)));
        }
    }
}
