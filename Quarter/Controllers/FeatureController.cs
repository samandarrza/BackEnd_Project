using Microsoft.AspNetCore.Mvc;
using Quarter.DAL;
using Quarter.Models;

namespace Quarter.Controllers
{
    public class FeatureController : Controller
    {
        private readonly QuarterDbContext _context;

        public FeatureController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Detail(int id)
        {
            var model = _context.Features.FirstOrDefault(x => x.Id == id);

            return View(model);
        }
    }
}
