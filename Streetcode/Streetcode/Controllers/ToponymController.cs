
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToponymController : ControllerBase 
    {
        private readonly IToponymService _toponymService;
        public ToponymController(IToponymService toponymService) 
        {
            _toponymService = toponymService;
        }
        [HttpPost("createToponym")]
        public void CreateToponym()
        {
            // TODO implement here
        }
        [HttpGet("getToponym")]
        public void GetToponym()
        {
            // TODO implement here
        }
        [HttpPut("updateToponym")]
        public void UpdateToponym()
        {
            // TODO implement here
        }
        [HttpDelete("deleteToponym")]
        public void DeleteToponym()
        {
            // TODO implement here
        }
        [HttpGet("getToponymByName")]
        public string GetToponymByName()
        {
            return _toponymService.GetToponymByNameAsync();
            // TODO implement here
        }
        [HttpGet("getStreetcodesByToponym")]
        public void GetStreetcodesByToponym() 
        {
            // TODO implement here
        }

    }
}