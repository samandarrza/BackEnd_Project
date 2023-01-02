using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quarter.DAL;
using Quarter.Helpers;
using Quarter.Models;
using System.Data;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

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
        [HttpPost]
        public IActionResult Create(Aminity aminity)
        {
            if (aminity == null)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Aminities.Any(x => x.Name == aminity.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.Aminities.Add(aminity);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Aminity aminity = _context.Aminities.FirstOrDefault(x => x.Id == id);
            if (aminity == null)
                return RedirectToAction("error", "dashboard");
            return View(aminity);
        }

        [HttpPost]
        public IActionResult Edit(Aminity aminity)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Aminities.Any(x => x.Name == aminity.Name && x.Id != aminity.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (aminity == null || aminity.Name == null || aminity.Icon == null)
            {
                return View();
            }
            Aminity existAminity = _context.Aminities.FirstOrDefault(x => x.Id == aminity.Id);
            if (existAminity == null)
            {
                return RedirectToAction("Index");
            }
            existAminity.Name = aminity.Name;
            existAminity.Icon = aminity.Icon;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Aminity aminity = _context.Aminities.FirstOrDefault(x => x.Id == id);

            if (aminity == null)
                return RedirectToAction("error", "dashboard");

            _context.Aminities.Remove(aminity);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
