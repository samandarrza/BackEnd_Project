using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quarter.DAL;
using Quarter.Models;
using System.Data;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class SettingController : Controller
    {
        private readonly QuarterDbContext _context;

        public SettingController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Settings.ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(x => x.Id == id);
            if (setting == null)
                return RedirectToAction("error", "dashboard");
            return View(setting);
        }

        [HttpPost]
        public IActionResult Edit(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_context.Settings.Any(x=>x.Key == setting.Key && x.Value == setting.Value))
            {
                ModelState.AddModelError("Value", "This Value is already taken");
                return View();
            }

            Setting existSetting = _context.Settings.FirstOrDefault(x => x.Key == setting.Key);
            if (existSetting == null)
            {
                return RedirectToAction("error","dashboard");
            }
            existSetting.Value = setting.Value;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
