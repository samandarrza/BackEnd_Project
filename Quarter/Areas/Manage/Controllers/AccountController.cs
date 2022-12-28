using Microsoft.AspNetCore.Mvc;
using Quarter.DAL;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly QuarterDbContext _context;

        public AccountController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
