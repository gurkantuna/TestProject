using BlogApp.Entity.Concrete;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers {
    [Authorize]
    public class AccountController : Controller {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string returnUrl) {

            if (ModelState.IsValid) {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);

                if (user! != null) {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, isPersistent: loginModel.RememberMe, false);

                    if (result.Succeeded) {
                        return Redirect(returnUrl ?? "/");
                    }
                }

                ModelState.AddModelError("Email", "Invalid email or password");
            }

            return View(loginModel);
        }
        
        public async Task<IActionResult> LogOut() {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}