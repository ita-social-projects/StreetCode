using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Streetcode.BLL.DTO.Email;
using Streetcode.BLL.MediatR.Email;

namespace Streetcode.WebApi.Controllers.Email
{
    public class EmailController : BaseApiController
    {
        [EnableRateLimiting("EmailRateLimit")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Send([FromBody] EmailDto email)
        {
            return HandleResult(await Mediator.Send(new SendEmailCommand(email)));
        }
    }
}
