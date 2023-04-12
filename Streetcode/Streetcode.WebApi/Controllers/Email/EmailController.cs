using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Email;
using Streetcode.BLL.MediatR.Email;

namespace Streetcode.WebApi.Controllers.Email
{
  public class EmailController : BaseApiController
  {
    [HttpPost]
    public async Task<IActionResult> Send([FromBody] EmailDTO email)
    {
      return HandleResult(await Mediator.Send(new SendEmailCommand(email)));
    }
  }
}
