using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Streetcode.WebApi.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // Admin Panel Access
        public IActionResult AdminPanel()
        {
            return View(); // Admin panel logic
        }

        // For Unauthorized Users
        [AllowAnonymous]
        public IActionResult UnauthorizedAccess()
        {
            return NotFound(); // Return 404 page for unauthorized users
        }
    }
}
