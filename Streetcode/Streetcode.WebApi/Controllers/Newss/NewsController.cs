using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.MediatR.Newss.Delete;
using Streetcode.BLL.MediatR.Newss.GetAll;
using Streetcode.BLL.MediatR.Newss.GetById;
using Streetcode.BLL.MediatR.Partners.Create;

namespace Streetcode.WebApi.Controllers.Newss
{
    public class NewsController : BaseApiController
    {
        [HttpPost]
        /*[AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]*/
        public async Task<IActionResult> Create([FromBody] NewsDTO partner)
        {
            return HandleResult(await Mediator.Send(new CreateNewsCommand(partner)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllNewsQuery()));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return HandleResult(await Mediator.Send(new GetNewsByIdQuery(id)));
        }

        [HttpDelete]
        /*[AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]*/
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteNewsCommand(id)));
        }
    }
}
