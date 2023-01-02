using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Cms;
using Quarter.DAL;
using Quarter.Models;
using System.Data;

namespace Quarter.Areas.Manage.Controllers
{
    namespace Quarter.Areas.Manage.Controllers
    {
        [Area("manage")]
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]

        public class CategoryController : Controller
        {
            private readonly QuarterDbContext _context;

            public CategoryController(QuarterDbContext context)
            {
                _context = context;
            }
            public IActionResult Index()
            {
                var model = _context.Categories.ToList();
                return View(model);
            }
            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            public IActionResult Create(Category category)
            {
                if (category == null)
                {
                    return View();
                }
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (_context.Categories.Any(x => x.Name == category.Name))
                {
                    ModelState.AddModelError("Name", "Bu adda data var");
                    return View();
                }

                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            public IActionResult Edit(int id)
            {
                Category category = _context.Categories.FirstOrDefault(x => x.Id == id);
                if (category == null)
                    return RedirectToAction("error", "dashboard");
                return View(category);
            }

            [HttpPost]
            public IActionResult Edit(Category category)
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (_context.Categories.Any(x => x.Name == category.Name && x.Id != category.Id))
                {
                    ModelState.AddModelError("Name", "Bu adda data var");
                    return View();
                }
                if (category == null || category.Name == null)
                {
                    return View();
                }
                Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
                if (existCategory == null)
                {
                    return RedirectToAction("Index");
                }
                existCategory.Name = category.Name;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            public IActionResult Delete(int id)
            {
                Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

                if (category == null)
                    return RedirectToAction("error", "dashboard");

                _context.Categories.Remove(category);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}
