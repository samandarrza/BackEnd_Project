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
        public IActionResult Edit(int id)
        {
            Broker broker = _context.Brokers.FirstOrDefault(x => x.Id == id);
            if (broker == null)
                return RedirectToAction("error", "dashboard");
            return View(broker);
        }
    }
}
