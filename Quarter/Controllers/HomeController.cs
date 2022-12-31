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
                Aminities = _context.Aminities.ToList(),
                Sliders = _context.Sliders.ToList(),
                Brokers = _context.Brokers.ToList(),
                Categories = _context.Categories.ToList(),
                Cities = _context.Cities.ToList(),
                Houses = _context.Houses.Include(x=>x.HouseImages).Include(x=>x.HouseAmenities).ThenInclude(x => x.Amenity).ToList(),
                Settings = _context.Settings.ToList(),
            };
            return View(homeVM);
        }
    }
}
