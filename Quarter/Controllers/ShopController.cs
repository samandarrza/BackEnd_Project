using Microsoft.AspNetCore.Mvc;

namespace Quarter.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
