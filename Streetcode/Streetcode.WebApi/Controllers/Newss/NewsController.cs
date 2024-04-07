using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.MediatR.Newss.Delete;
using Streetcode.BLL.MediatR.Newss.GetAll;
using Streetcode.BLL.MediatR.Newss.GetById;
using Streetcode.BLL.MediatR.Newss.GetByUrl;
using Streetcode.BLL.MediatR.Newss.GetNewsAndLinksByUrl;
using Streetcode.BLL.MediatR.Newss.SortedByDateTime;
using Streetcode.BLL.MediatR.Newss.Update;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Newss
{
    public class NewsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllNewsQuery()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return HandleResult(await Mediator.Send(new GetNewsByIdQuery(id)));
        }

        [HttpGet("{url}")]
        public async Task<IActionResult> GetByUrl(string url)
        {
            return HandleResult(await Mediator.Send(new GetNewsByUrlQuery(url)));
        }

        [HttpGet("{url}")]
        public async Task<IActionResult> GetNewsAndLinksByUrl(string url)
        {
            return HandleResult(await Mediator.Send(new GetNewsAndLinksByUrlQuery(url)));
        }

        [HttpGet]
        public async Task<IActionResult> SortedNewsByDateTime()
        {
            return HandleResult(await Mediator.Send(new SortedByDateTimeQuery()));
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] NewsCreateDTO partner)
        {
            return HandleResult(await Mediator.Send(new CreateNewsCommand(partner)));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteNewsCommand(id)));
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(NewsDTO newsDTO)
        {
            return HandleResult(await Mediator.Send(new UpdateNewsCommand(newsDTO)));
        }
    }
}
