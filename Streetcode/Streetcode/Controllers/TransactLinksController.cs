
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactLinksController : ControllerBase 
{
    private readonly ITransactLinksService _transactLinksService;
    public TransactLinksController(ITransactLinksService transactLinksService) 
    {
        _transactLinksService = transactLinksService;
    }

    [HttpPost("createTransactLink")]
    public void CreateTransactLink()
    {
        // TODO implement here
    }
    [HttpGet("getTransactLink")]
    public string GetTransactLink()
    {
        return _transactLinksService.GetTransactLinkAsync();
        // TODO implement here
    }
    [HttpPost("updateTransactLink")]
    public void UpdateTransactLink()
    {
        // TODO implement here
    }
    [HttpDelete("deleteTransactLink")]
    public void DeleteTransactLink() 
    {
        // TODO implement here
    }

}