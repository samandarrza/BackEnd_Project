using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Quarter.Areas.Manage.ViewModels;
using Quarter.DAL;
using Quarter.Models;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    public class HouseController : Controller
    {
        private readonly QuarterDbContext _context;

        public HouseController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Houses
            .Include(x => x.Category)
            .Include(x => x.HouseAmenities).ThenInclude(x=>x.Amenity)
            .Include(x => x.City)
            .Include(x=>x.Broker);

            var model = PaginatedList<House>.Create(query, page, 4);
            return View(model);
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
