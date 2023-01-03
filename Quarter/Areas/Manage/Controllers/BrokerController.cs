using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;
using Quarter.DAL;
using Quarter.Helpers;
using Quarter.Models;
using System.Data;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class BrokerController : Controller
    {
        private readonly QuarterDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BrokerController(QuarterDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.Brokers.Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPage = (int)Math.Ceiling(_context.Sliders.Count() / 5d);
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Broker broker)
        {
            if (broker == null)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Brokers.Any(x => x.Fullname == broker.Fullname))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }

            if (broker.ProfilFile != null)
            {
                broker.Image = FileManager.Save(broker.ProfilFile, _env.WebRootPath, "main/uploads/brokerImage");
            }

            _context.Brokers.Add(broker);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Broker broker = _context.Brokers.FirstOrDefault(x => x.Id == id);
            if (broker == null)
                return RedirectToAction("error", "dashboard");
            return View(broker);
        }
        [HttpPost]
        public IActionResult Edit(Broker broker)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Brokers.Any(x => x.Fullname == broker.Fullname && x.Id != broker.Id))
            {
                ModelState.AddModelError("Name", "Bu adda data var");
                return View();
            }
            if (broker == null)
            {
                return View();
            }
            Broker existBroker = _context.Brokers.FirstOrDefault(x => x.Id == broker.Id);
            if (existBroker == null)
            {
                return RedirectToAction("Index");
            }
            if (broker.ProfilFile != null)
            {
                var newImage = FileManager.Save(broker.ProfilFile, _env.WebRootPath, "main/uploads/brokerImage");
                FileManager.Delete(_env.WebRootPath, "main/uploads/brokerImage", existBroker.Image);
                existBroker.Image = newImage;
            }
            existBroker.Fullname = broker.Fullname;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Broker broker = _context.Brokers.FirstOrDefault(x => x.Id == id);

            if (broker == null)
                return RedirectToAction("error", "dashboard");

            if (broker.Image != null)
            {
                FileManager.Delete(_env.WebRootPath, "main/uploads/brokerImage", broker.Image);
            }
            _context.Brokers.Remove(broker);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
