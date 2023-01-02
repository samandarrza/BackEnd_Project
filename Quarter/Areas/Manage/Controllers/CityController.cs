using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quarter.DAL;
using Quarter.Models;
using System.Data;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class CityController : Controller
    {
        private readonly QuarterDbContext _context;

        public CityController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Cities.ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(City city)
        {
            if (city == null)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Cities.Any(x => x.Name == city.Name))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            _context.Cities.Add(city);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            City city = _context.Cities.FirstOrDefault(x => x.Id == id);
            if (city == null)
                return RedirectToAction("error", "dashboard");
            return View(city);
        }

        [HttpPost]
        public IActionResult Edit(City city)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Cities.Any(x => x.Name == city.Name && x.Id != city.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (city == null || city.Name == null)
            {
                return View();
            }
            City existCity = _context.Cities.FirstOrDefault(x => x.Id == city.Id);
            if (existCity == null)
            {
                return RedirectToAction("Index");
            }
            existCity.Name = city.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            City city = _context.Cities.FirstOrDefault(x => x.Id == id);

            if (city == null)
                return RedirectToAction("error", "dashboard");

            _context.Cities.Remove(city);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
