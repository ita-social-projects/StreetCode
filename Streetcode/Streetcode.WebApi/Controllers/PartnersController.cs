
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Interfaces.Partners;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartnersController : ControllerBase 
{
    private readonly IPartnersService _partnesService;
    public PartnersController(IPartnersService prtnersService) 
    {
        _partnesService = prtnersService;
    }
    [HttpPost("createPartner")]
    public void CreatePartner() 
    {
        // TODO implement here
    }
    [HttpGet("getPartner")]
    public void GetPartner() 
    {
        // TODO implement here
    }
    [HttpPut("updatePartner")]
    public void UpdatePartner()
    {
        // TODO implement here
    }
    [HttpDelete("deletePartner")]
    public void DeletePartner() 
    {
        // TODO implement here
    }
    [HttpGet("getPartnerById")]
    public void GetPartnerById() 
    {
        // TODO implement here
    }
    [HttpGet("getSponsors")]
    public string GetSponsors() 
    {
        return _partnesService.GetSponsorsAsync();
        // TODO implement here
    }

}