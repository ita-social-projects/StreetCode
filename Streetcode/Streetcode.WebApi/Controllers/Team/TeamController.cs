using Microsoft.AspNetCore.Mvc;

namespace Streetcode.WebApi.Controllers.Team
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
