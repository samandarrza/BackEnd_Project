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


        public IActionResult Edit(int id)
        {
            About about = _context.Abouts.FirstOrDefault(x => x.Id == id);
            if (about == null)
                return RedirectToAction("error", "dashboard");
            return View(about);
        }
        [HttpPost]
        public IActionResult Edit(About about)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Abouts.Any(x => x.Title == about.Title && x.Id != about.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (about == null)
            {
                return View();
            }
            About existAbout = _context.Abouts.FirstOrDefault(x => x.Id == about.Id);
            if (existAbout == null)
            {
                return RedirectToAction("Index");
            }
            if (about.ImageFile != null)
            {
                var newImage = FileManager.Save(about.ImageFile, _env.WebRootPath, "main/uploads/brokerImage");
                FileManager.Delete(_env.WebRootPath, "main/uploads/brokerImage", existAbout.Image);
                existAbout.Image = newImage;
            }
            if (about.VideoImageFile != null)
            {
                var newImage = FileManager.Save(about.VideoImageFile, _env.WebRootPath, "main/uploads/aboutImage");
                FileManager.Delete(_env.WebRootPath, "main/uploads/aboutImage", existAbout.VideoImage);
                existAbout.VideoImage = newImage;
            }
            existAbout.Title = about.Title;
            existAbout.Desc = about.Desc;
            existAbout.Text = about.Text;
            existAbout.BtnUrl = about.BtnUrl;
            existAbout.BtnText = about.BtnText;
            existAbout.VideoUrl = about.VideoUrl;


            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            About about = _context.Abouts.FirstOrDefault(x => x.Id == id);

            if (about == null)
                return RedirectToAction("error", "dashboard");

            if (about.Image != null)
            {
                FileManager.Delete(_env.WebRootPath, "main/uploads/aboutImage", about.Image);
            }
            if (about.VideoImage != null)
            {
                FileManager.Delete(_env.WebRootPath, "main/uploads/aboutImage", about.VideoImage);
            }
            _context.Abouts.Remove(about);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
