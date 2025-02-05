using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Streetcode.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetProfile()
        {
            var currentUser = User.Identity.Name; // Or use user ID to identify

            if (User.IsInRole("Admin"))
            {
                return NotFound(); // Return 404 if admin attempts to access their profile
            }

            // Logic for retrieving the profile goes here
            return Ok("Profile information");
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateProfile([FromBody] string newProfileInfo)
        {
            var currentUser = User.Identity.Name;

            if (User.IsInRole("Admin"))
            {
                return NotFound(); // Return 404 if admin attempts to update their profile
            }

            // Logic for updating the profile goes here
            return Ok("Profile updated successfully");
        }
    }
}
