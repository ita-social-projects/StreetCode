
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;


namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FactController : ControllerBase 
    {

        private readonly IFactService _factService;

        public FactController(IFactService factService) 
        {
            _factService = factService;
        }

        [HttpGet("getFactById")]
        public void GetFactById() 
        {
            // TODO implement here
        }
        [HttpGet("getFactByStreetcode")]
        public void GetFactsByStreetcode() 
        {
            // TODO implement here
        }
        [HttpPost("createFact")]
        public void CreateFact() 
        {
            // TODO implement here
        }
        [HttpGet("getFact")]
        public string GetFact()
        {
            // TODO implement here
            return _factService.GetFactsByStreetcode();
        }
        [HttpPut("updateFact")]
        public void UpdateFact() 
        {
            // TODO implement here
        }
        [HttpDelete("deleteFact")]
        public void DeleteFactById() 
        {
            // TODO implement here
        }

    }
}