using Microsoft.AspNetCore.Mvc;

namespace Quarter.Controllers
{
    public class HouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
