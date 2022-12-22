using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Interfaces.Streetcode;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class StreetcodeController : ControllerBase
{
    private readonly IStreetcodeService _streetcodeService;
    public StreetcodeController(IStreetcodeService streetcodeService)
    {
        _streetcodeService = streetcodeService;
    }

    [HttpPost]
    [Route("createStreetcode")]
    public void CreateStreetcode()
    {
        // TODO implement here
    }

    [HttpGet("getStreetcode")]
    public void GetStreetcode()
    {
        // TODO implement here
    }

    [HttpPut("updateStreetcode")]
    public void UpdateStreetcode()
    {
        // TODO implement here
    }

    [HttpDelete("deleteStreetcode")]
    public void DeleteStreetcode()
    {
        // TODO implement here
    }

    [HttpGet("getStreetcodeByName")]
    public void GetStreetcodeByName()
    {
         // TODO implement here
    }

    [HttpGet("getStreetcodesByTagA")]
    public void GetStreetcodesByTag()
    {
        // TODO implement here
    }

    [HttpGet("getByCode")]
    public void GetByCode()
    {
        // TODO implement here
    }

    [HttpGet("getTagsByStreecodeId")]
    public void GetTagsByStreecodeId()
    {
        // TODO implement here
    }

    [HttpGet("getEvents")]
    public void GetEvents()
    {
        // TODO implement here
    }

    [HttpGet("getPersons")]
    public void GetPersons()
    {
        // TODO implement here
    }
}