using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quarter.DAL;
using Quarter.Helpers;
using Quarter.Models;
using System.Data;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class FeatureController : Controller
    {
        private readonly QuarterDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FeatureController(QuarterDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var model = _context.Features.ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Feature feature)
        {
            if (feature == null)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Features.Any(x => x.Title == feature.Title))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            if (feature.ImageFile != null)
            {
                feature.Image = FileManager.Save(feature.ImageFile, _env.WebRootPath, "main/uploads/featureImage");
            }

            _context.Features.Add(feature);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null)
                return RedirectToAction("error", "dashboard");
            return View(feature);
        }
        [HttpPost]
        public IActionResult Edit(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Features.Any(x => x.Title == feature.Title && x.Id != feature.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (feature == null)
            {
                return View();
            }
            Feature existFeature = _context.Features.FirstOrDefault(x => x.Id == feature.Id);
            if (existFeature == null)
            {
                return RedirectToAction("Index");
            }
            if (feature.ImageFile != null)
            {
                var newImage = FileManager.Save(feature.ImageFile, _env.WebRootPath, "main/uploads/featureImage");
                FileManager.Delete(_env.WebRootPath, "main/uploads/featureImage", existFeature.Image);
                existFeature.Image = newImage;
            }
            existFeature.Title = feature.Title;
            existFeature.Desc = feature.Desc;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == id);

            if (feature == null)
                return RedirectToAction("error", "dashboard");

            _context.Features.Remove(feature);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
