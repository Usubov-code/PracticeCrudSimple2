using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracticeCrudSimple.Data;
using PracticeCrudSimple.Models;
using PracticeCrudSimple.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeCrudSimple.Areas.admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;

        public AccountController(AppDbContext context,UserManager<CustomUser> userManager,SignInManager<CustomUser> signInManager)
        {
            _context = context;
           _signInManager = signInManager;
            _userManager = userManager;
        }
        
        
        public IActionResult Register()
        {


            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Register(VmRegister model)
        {
           
            if (!ModelState.IsValid)
            {

                return View();
            }

            else
            {
              
                if (_context.CustomUsers.Any(c=>c.UserName==model.UserName))
                {
                    ModelState.AddModelError("UserName", "Hal hazirda movcuddur!");
                    return View(model);
                }
                
                else
                {
                    if (_context.CustomUsers.Any(c => c.Email == model.Email))
                    {
                        ModelState.AddModelError("Email", "Hal hazirda movcuddur!");
                        return View(model);
                    }
                    CustomUser user = new CustomUser()
                    {

                        FullName = model.FullName,
                        UserName = model.UserName,
                        Email = model.Email
                    };

                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, false);
                            return RedirectToAction("login", "account");
                        }
                        else
                        {

                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", "Password da ");
                            }

                        }
                    
                    return View(model);
                }


            }
            
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VmLogin model)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }
            CustomUser member = _userManager.Users.FirstOrDefault(x => x.NormalizedEmail == model.Email.ToUpper());

            if(member == null)
            {
                ModelState.AddModelError("", "Member yoxdur");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                foreach (var item in result )
                {

                }
            }

            return RedirectToAction();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
    }
}
