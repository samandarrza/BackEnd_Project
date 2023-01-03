using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quarter.Areas.Manage.ViewModels;
using Quarter.DAL;
using Quarter.Models;
using System.Data;

namespace Quarter.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [Area("manage")]
    public class OrderController : Controller
    {
        private readonly QuarterDbContext _context;

        public OrderController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Orders.Include(x => x.OrderItems);

            var model = PaginatedList<Order>.Create(query, page, 4);

            return View(model);
        }
        public IActionResult Edit(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return RedirectToAction("error", "dashboard");

            return View(order);
        }

        [HttpPost]
        public IActionResult Accept(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return RedirectToAction("error", "dashboard");

            order.Status = Enums.OrderStatus.Accepted;

            foreach (var item in order.OrderItems)
            {
                House house = _context.Houses.FirstOrDefault(x => x.Id == item.HouseId);

                house.Status = false;
            }

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Reject(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return RedirectToAction("error", "dashboard");

            order.Status = Enums.OrderStatus.Rejected;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
