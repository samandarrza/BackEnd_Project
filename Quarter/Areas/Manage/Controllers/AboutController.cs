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

    public class AboutController : Controller
    {
        private readonly QuarterDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AboutController(QuarterDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var model = _context.Abouts.ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(About about)
        {
            if (about == null)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Abouts.Any(x => x.Title == about.Title))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            if (about.ImageFile != null)
            {
                about.Image = FileManager.Save(about.ImageFile, _env.WebRootPath, "main/uploads/aboutImage");
            }
            if (about.VideoImageFile != null)
            {
                about.VideoImage = FileManager.Save(about.VideoImageFile, _env.WebRootPath, "main/uploads/aboutImage");
            }

            _context.Abouts.Add(about);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }




        public IActionResult Edit()
        {
            return View();
        }
    }
}
