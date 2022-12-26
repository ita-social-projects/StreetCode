using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.WebApi.Controllers.Partners;

public class PartnersController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        // TODO implement here
        return Ok(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PartnerDTO partner)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PartnerDTO partner)
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