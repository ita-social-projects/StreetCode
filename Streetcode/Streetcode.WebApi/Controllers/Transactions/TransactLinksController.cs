using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetAll;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetById;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Transactions;

public class TransactLinksController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllTransactLinksQuery()));
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetTransactLinkByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetTransactLinkByIdQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TransactLinkDTO transactLink)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TransactLinkDTO transactLink)
    {
        // TODO implement here
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        // TODO implement here
        return Ok();
    }
}