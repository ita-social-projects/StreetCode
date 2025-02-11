using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.MediatR.Newss.Delete;
using Streetcode.BLL.MediatR.Newss.GetAll;
using Streetcode.BLL.MediatR.Newss.GetById;
using Streetcode.BLL.MediatR.Newss.GetByUrl;
using Streetcode.BLL.MediatR.Newss.GetNewsAndLinksByUrl;
using Streetcode.BLL.MediatR.Newss.Update;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Newss
{
    public class NewsController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NewsDTO>))]
        public async Task<IActionResult> GetAll([FromQuery] ushort page = 1, [FromQuery] ushort pageSize = 10)
        {
            return HandleResult(await Mediator.Send(new GetAllNewsQuery(page, pageSize)));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NewsDTO>))]
        public async Task<IActionResult> GetAllPublished([FromQuery] ushort page = 1, [FromQuery] ushort pageSize = 10)
        {
            return HandleResult(await Mediator.Send(new GetAllNewsQuery(page, pageSize, DateTime.UtcNow)));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewsDTO))]
        public async Task<IActionResult> GetById(int id)
        {
            return HandleResult(await Mediator.Send(new GetNewsByIdQuery(id)));
        }

        [HttpGet("{url}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewsDTO))]
        public async Task<IActionResult> GetByUrl(string url)
        {
            return HandleResult(await Mediator.Send(new GetNewsByUrlQuery(url)));
        }

        [HttpGet("{url}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewsDTOWithURLs))]
        public async Task<IActionResult> GetNewsAndLinksByUrl(string url)
        {
            return HandleResult(await Mediator.Send(new GetNewsAndLinksByUrlQuery(url)));
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NewsDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] NewsCreateDTO partner)
        {
            return HandleResult(await Mediator.Send(new CreateNewsCommand(partner)));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteNewsCommand(id)));
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateNewsDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(UpdateNewsDTO newsDto)
        {
            return HandleResult(await Mediator.Send(new UpdateNewsCommand(newsDto)));
        }
    }
}
