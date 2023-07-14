using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetAllByTermId;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Update;

namespace Streetcode.WebApi.Controllers.Streetcode.TextContent
{
    public class RelatedTermController : BaseApiController
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByTermId([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new GetAllRelatedTermsByTermIdQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RelatedTermDTO relatedTerm)
        {
            return HandleResult(await Mediator.Send(new CreateRelatedTermCommand(relatedTerm)));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RelatedTermDTO relatedTerm)
        {
            return HandleResult(await Mediator.Send(new UpdateRelatedTermCommand(id, relatedTerm)));
        }

        [HttpDelete("{word}")]
        public async Task<IActionResult> Delete([FromRoute] string word)
        {
            return HandleResult(await Mediator.Send(new DeleteRelatedTermCommand(word)));
        }
    }
}
