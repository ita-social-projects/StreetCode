using Microsoft.AspNetCore.Mvc;

namespace Streetcode.WebApi.Controllers.Users
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
