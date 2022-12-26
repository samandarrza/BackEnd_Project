using Microsoft.AspNetCore.Mvc;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
