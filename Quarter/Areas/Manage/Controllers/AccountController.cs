﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quarter.Areas.Manage.ViewModels;
using Quarter.DAL;
using Quarter.Models;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVM, string returnUrl)
        {
            AppUser user = await _userManager.FindByNameAsync(adminVM.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect!");
                return View();
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("SuperAdmin") && !roles.Contains("Admin") && !roles.Contains("Editor"))
            {
                ModelState.AddModelError("", "Username or Password is incorrect!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, adminVM.Password, adminVM.IsPersist, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "You are locked out, please wait 5 minute begore trying again");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorrect!");
                return View();
            }
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("index", "dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("login", "account");
        }

        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole role1 = new IdentityRole("Member");
        //    IdentityRole role2 = new IdentityRole("SuperAdmin");
        //    IdentityRole role3 = new IdentityRole("Admin");
        //    IdentityRole role4 = new IdentityRole("Editor");
        //    await _roleManager.CreateAsync(role1);
        //    await _roleManager.CreateAsync(role2);
        //    await _roleManager.CreateAsync(role3);
        //    await _roleManager.CreateAsync(role4);
        //    return Ok();
        //}

        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser
        //    {
        //        UserName = "semenderadmin",
        //        FullName = "Semender Rzayev"
        //    };

        //    await _userManager.CreateAsync(admin, "Admin123@");
        //    await _userManager.AddToRoleAsync(admin, "SuperAdmin");

        //    return Ok();
        //}


    } 
}
