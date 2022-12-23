using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Partners;

namespace Streetcode.WebApi.Controllers.Partners;

[ApiController]
[Route("api/[controller]/[action]")]
public class PartnersController : ControllerBase
{
    private readonly IPartnersService _partnesService;
    public PartnersController(IPartnersService prtnersService)
    {
        _partnesService = prtnersService;
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
        return Ok(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PartnerDTO partner)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(PartnerDTO partner)
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

    [HttpGet]
    public async Task<IActionResult> GetSponsors()
    {
        // TODO implement here
        return Ok();
    }
}