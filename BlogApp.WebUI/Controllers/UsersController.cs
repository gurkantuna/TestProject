using BlogApp.DataAccess.Abstract;
using BlogApp.Entity.Concrete;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Controllers {

    [Authorize(Roles = "Admin")]
    public class UsersController : Controller {

        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRolesDal _userRolesManager;

        public UsersController(UserManager<AppUser> userManager, IUserRolesDal userRolesDal) {
            _userManager = userManager;
            _userRolesManager = userRolesDal;
        }

        public IActionResult Index() {
            var users = _userManager.Users;
            return View(users);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterUserModel registerModel) {

            if (ModelState.IsValid) {
                var user = new AppUser {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email

                };

                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerModel);
        }

        public async Task<IActionResult> Update(string id) {
            var user = await _userManager.FindByIdAsync(id);

            //TODO : Parola sıfırlama için ayrı bir view daha oluşturulabilir.
            var updateModel = new UpdateUserModel {
                Id = id,
                Email = user.Email,
                UserName = user.UserName
            };
            return View(updateModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateUserModel model) {

            if (ModelState.IsValid) {

                var user = await _userManager.FindByIdAsync(model.Id);

                if (user != null) {
                    user.Email = model.Email;
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded) {
                        return RedirectToAction("Index");
                    }
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id) {

            if (string.IsNullOrWhiteSpace(id)) {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(id);
            var userRoles = _userRolesManager.GetUserRoles(user.Id);

            if (userRoles.Any()) {
                foreach (var item in userRoles) {
                    _userRolesManager.DeleteUserRoles(item.UserId, item.RoleId);
                }
            }

            if (user != null) {
                await _userManager.DeleteAsync(user);
            }

            return Json(user);
        }
    }
}