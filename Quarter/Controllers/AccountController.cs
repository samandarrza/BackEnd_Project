using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles ="Member")]
        public async Task<IActionResult> Index()
        {
            AppUser user =  await _userManager.FindByNameAsync(User.Identity.Name);

            MemberUpdateViewModel memberVM = new MemberUpdateViewModel
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Image = user.Image,
                Email = user.Email,
            };
            return View(memberVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(MemberUpdateViewModel memberVM)
        {
            return Ok(memberVM);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberVM, string returnUrl)
        {
            AppUser user = await _userManager.FindByNameAsync(memberVM.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }
                
            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("Member"))
            {
                ModelState.AddModelError("", "Username or Password is incorrect!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, memberVM.Password, false, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }

            if (returnUrl != null)
                return Redirect(returnUrl);

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
            await _userManager.AddToRoleAsync(user, "Member");

            return RedirectToAction("login");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }
    }
}
