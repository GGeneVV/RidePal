using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RidePal.Models;
using RidePal.Services.Contracts;
using RidePal.Web.Models;
using System;
using System.Threading.Tasks;

namespace RidePal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            var vm = new RegisterVM();
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("Account/SignUp")]
        public async Task<IActionResult> SignUp(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = model.UserName,
                    NormalizedUserName = model.UserName.Normalize(),
                    Email = model.Email,
                    CreatedOn = DateTime.Now,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public JsonResult IsAlreadySigned(string userName)
        {

            return Json(IsUserAvailable(userName));

        }
        public async Task<bool> IsUserAvailable(string userName)
        {
            // Assume these details coming from database  
            var RegUser = await _userManager.FindByNameAsync(userName);

            bool status;
            if (RegUser != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }

            return status;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            //var vm = new LoginViewModel();
            //return View(vm);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _signInManager.SignOutAsync();

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
