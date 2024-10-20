using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetAll;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetById;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Transactions;

public class TransactLinksController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TransactLinkDTO>))]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllTransactLinksQuery()));
    }

    [HttpGet("{streetcodeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactLinkDTO))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        var res = await Mediator.Send(new GetTransactLinkByStreetcodeIdQuery(streetcodeId));
        return HandleResult(res);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactLinkDTO))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetTransactLinkByIdQuery(id)));
    }
}