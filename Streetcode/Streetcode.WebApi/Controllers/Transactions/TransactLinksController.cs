using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.Transactions;

namespace Streetcode.WebApi.Controllers.Transactions;

[ApiController]
[Route("api/[controller]/[action]")]
public class TransactLinksController : ControllerBase
{
    private readonly ITransactLinksService _transactLinksService;
    public TransactLinksController(ITransactLinksService transactLinksService)
    {
        _transactLinksService = transactLinksService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO implement here
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TransactLinkDTO transactLink)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(TransactLinkDTO transactLink)
    {
        // TODO implement here
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // TODO implement here
        return Ok();
    }
}