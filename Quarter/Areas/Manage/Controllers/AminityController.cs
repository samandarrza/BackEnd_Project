using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quarter.DAL;
using Quarter.Models;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AminityController : Controller
    {
        private readonly QuarterDbContext _context;

        public AminityController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Aminities.ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            Aminity aminity = _context.Aminities.FirstOrDefault(x => x.Id == id);
            if (aminity == null)
                return RedirectToAction("error", "dashboard");
            return View(aminity);
        }
    }
}
