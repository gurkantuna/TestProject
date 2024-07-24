﻿using BlogApp.DataAccess.EntitFrameworkCore.Abstract;
using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers {
    public class CategoryController : Controller {

        public CategoryController(ICategoryRepo categoryRepo) {
            _categoryRepo = categoryRepo;
        }

        private readonly ICategoryRepo _categoryRepo;

        public IActionResult Index() {
            var categories = _categoryRepo.GetList();
            return View(categories);
        }

        public IActionResult Create() {
            return View(new Category());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Category category) {
            if (ModelState.IsValid) {
                _categoryRepo.Add(category);
                return RedirectToAction("Index", "Category");
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int? id) {
            var category = _categoryRepo.Get(x => x.Id == id);
            return View(category);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public IActionResult Update(Category category) {
            _categoryRepo.Update(category);
            return RedirectToAction("List", "Article");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id) {

            if (id == null) {
                return BadRequest();
            }

            var category = _categoryRepo.Get(x => x.Id == id);
            _categoryRepo.Delete(category);
            return Json(category);
        }
    }
}