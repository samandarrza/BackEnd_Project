using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quarter.DAL;
using Quarter.Helpers;
using Quarter.Models;
using Quarter.ViewModels;
using System.Drawing;
using System.Xml.Linq;

namespace Quarter.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuarterDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(QuarterDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberVM)
        {
            AppUser user = await _userManager.FindByNameAsync(memberVM.UserName);

            if (user == null)
                ModelState.AddModelError("", "Username or Password is incorrect");

            var result = await _signInManager.PasswordSignInAsync(user, memberVM.Password, false, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }

            return RedirectToAction("index", "home");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterVewModel memberVM)
        {
            if (!ModelState.IsValid)
                return View();

            if (await _userManager.FindByNameAsync(memberVM.UserName) != null)
            {
                ModelState.AddModelError("Username", "Username already exist!");
                return View();
            }
            if (await _userManager.FindByEmailAsync(memberVM.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exist!");
                return View();
            }

            AppUser user = new AppUser
            {
                UserName = memberVM.UserName,
                Email = memberVM.Email,
                FullName = memberVM.FullName,
            };

            if (memberVM.ImageFile != null)
            {
                user.Image = FileManager.Save(memberVM.ImageFile, _env.WebRootPath, "main/uploads/userImage");
            }

            var result = await _userManager.CreateAsync(user,memberVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            return RedirectToAction("login");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }
    }
}
