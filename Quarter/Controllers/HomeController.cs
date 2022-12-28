using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quarter.DAL;
using Quarter.ViewModels;

namespace Quarter.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuarterDbContext _context;

        public HomeController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel
            {
               Sliders = _context.Sliders.ToList(),
               Aminities = _context.Aminities.ToList(),
            };
            return View(homeVM);
        }
    }
}
