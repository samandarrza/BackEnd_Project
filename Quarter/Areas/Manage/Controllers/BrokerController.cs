using Microsoft.AspNetCore.Mvc;
using Quarter.DAL;
using Quarter.Models;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    public class BrokerController : Controller
    {
        private readonly QuarterDbContext _context;

        public BrokerController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Brokers.ToList();
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
            if (broker == null || broker.Fullname == null || broker.Image == null)
            {
                return View();
            }
            Broker existBroker = _context.Brokers.FirstOrDefault(x => x.Id == broker.Id);
            if (existBroker == null)
            {
                return RedirectToAction("Index");
            }
            existBroker.Fullname = broker.Fullname;
            existBroker.Image = broker.Image;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
