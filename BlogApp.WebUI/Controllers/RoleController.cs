using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApp.Entity.Concrete;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers {
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller {

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public IActionResult Index() {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name) {

            if (ModelState.IsValid) {

                var role = new IdentityRole(name);

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(name);
        }

        public async Task<IActionResult> Update(string id) {
            var role = await _roleManager.FindByIdAsync(id);

            var alreadyMembers = new List<AppUser>();
            var toMembers = new List<AppUser>();

            foreach (var user in _userManager.Users) {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? alreadyMembers : toMembers;
                list.Add(user);
            }

            var model = new RoleDetailModel {
                Role = role,
                AlreadyMembers = alreadyMembers,
                ToMembers = toMembers
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(RoleEditModel model) {
            IdentityResult result;

            if (ModelState.IsValid) {

                foreach (var userId in model.IdsToAdd ?? []) {
                    var user = await _userManager.FindByIdAsync(userId);

                    if (user != null) {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);

                        if (!result.Succeeded) {
                            foreach (var error in result.Errors) {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }

                }

                foreach (var userId in model.IdsToDelete ?? new string[] { }) {
                    var user = await _userManager.FindByIdAsync(userId);

                    if (user != null) {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

                        if (!result.Succeeded) {
                            foreach (var error in result.Errors) {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }

                }
            }

            if (ModelState.IsValid) {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Update", model.RoleId);
        }

        public async Task<IActionResult> Delete(string id) {

            if (string.IsNullOrWhiteSpace(id)) {
                return BadRequest();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role != null) {
                await _roleManager.DeleteAsync(role);
            }

            return Json(role);
        }
    }
}