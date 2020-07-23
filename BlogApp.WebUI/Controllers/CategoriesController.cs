
using BlogApp.Core.Constants;
using BlogApp.DataAccess.Abstract;
using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers {
    public class CategoriesController : Controller {
        private readonly ICategoryDal _categoryDal;

        public CategoriesController(ICategoryDal categoryDal) {
            _categoryDal = categoryDal;
        }

        public IActionResult Index() {
            var categories = _categoryDal.GetList();
            return View(categories);
        }

        public IActionResult Create() {
            return View(new Category());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Category category) {
            if (ModelState.IsValid) {
                _categoryDal.Add(category);
                return RedirectToAction("Index", "Categories");
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int? id) {
            var category = _categoryDal.Get(x => x.Id == id);
            return View(category);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public IActionResult Update(Category category) {
            _categoryDal.Update(category);
            return RedirectToAction("List", "Blogs");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id) {

            if (id == null) {
                return BadRequest();
            }

            var category = _categoryDal.Get(x => x.Id == id);
            _categoryDal.Delete(category);
            return Json(category);
        }
    }
}