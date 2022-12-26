using Microsoft.AspNetCore.Mvc;

namespace Quarter.Areas.Manage.Controllers
{
    public class HouseController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
