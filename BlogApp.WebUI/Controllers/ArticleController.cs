using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using BlogApp.Core.Constants;
using BlogApp.DataAccess.EntitFrameworkCore.Abstract;
using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.WebUI.Controllers {

    [Authorize]
    public class ArticleController : Controller {

        public ArticleController(IArticleRepo blogRepo, ICategoryRepo categoryRepo, ILogRepo logRepo) {
            _articleRepo = blogRepo;
            _categoryRepo = categoryRepo;
            _logRepo = logRepo;
        }

        private readonly IArticleRepo _articleRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly ILogRepo _logRepo;

        public ActionResult Index() {
            return View();
        }

        [Route("/Operations")]
        public IActionResult List() {

            var log = new Log {
                Audit = "INFO",
                Date = DateTime.Now,
                Detail = "Home.Index()",
                User = User.Identity.IsAuthenticated ? User.Identity.Name : Strings.Anonymous,
                Ip = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logRepo.Add(log);

            var articles = _articleRepo.GetList();
            return View(articles);
        }

        public IActionResult Create() {
            ViewBag.Categories = new SelectList(_categoryRepo.GetList(), "Id", "Name");
            return View(new Article());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article blog, IFormFile file) {

            if (file?.Length > 5000000) {
                ModelState.AddModelError(Strings.FileSize, Strings.FileLimit);
            }

            if (file != null && !file.ContentType.Contains(Strings.Image)) {
                ModelState.AddModelError(Strings.WrongType, Strings.ImageError);
            }

            if (ModelState.IsValid) {
                if (file != null) {
                    await UploadImage(blog, file);
                }
                else {
                    blog.Image = "dummy.jpg";
                    blog.ImageThumb = "dummy_thumb.jpg";
                }

                if (string.IsNullOrWhiteSpace(blog.Body)) {
                    blog.Body = Strings.NonBodyInArticle;
                }

                blog.Date = DateTime.Now;
                _articleRepo.Add(blog);
                return RedirectToAction("List", "Article");
            }
            ViewBag.Categories = new SelectList(_categoryRepo.GetList(), "Id", "Name");
            return View(blog);
        }

        private async Task UploadImage(Article blog, IFormFile file) {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img", file.FileName);
            var fileInfo = new FileInfo(file.FileName);
            var fileName = file.FileName.Remove(file.FileName.IndexOf(fileInfo.Extension));
            var thumbFileName = $"{fileName}_thumb{fileInfo.Extension}";
            var pathThumb = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img", thumbFileName);

            await CopyToStreamImage(blog, file, path);
            var resizeImg = ResizeImage(path, 150);
            resizeImg.Save(pathThumb);
            blog.ImageThumb = thumbFileName;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int? id) {
            var blog = _articleRepo.Get(x => x.Id == id);
            ViewBag.Categories = new SelectList(_categoryRepo.GetList(), "Id", "Name");
            return View(blog);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Article blog, IFormFile file) {
            if (ModelState.IsValid) {
                blog.Date = DateTime.Now;
                if (file != null) {
                    await UploadImage(blog, file);
                }

                _articleRepo.Update(blog);
                TempData["message"] = $"{blog.Title} updated";
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_categoryRepo.GetList(), "Id", "Name");
            return View(blog);
        }

        // TODO : Benzer işler Business'a taşınmalı
        private async Task CopyToStreamImage(Article blog, IFormFile file, string path) {
            await using (var stream = new FileStream(path, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }

            blog.Image = file.FileName;
        }

        // TODO : Benzer işler Business'a taşınmalı
        public static Bitmap ResizeImage(string fileName, int width) {
            Image sourceImage = Image.FromFile(fileName);
            float ratio = (float)sourceImage.Width / width;
            float height = sourceImage.Height / ratio;

            SizeF newSize = new SizeF(width, height);
            Bitmap target = new Bitmap(sourceImage, (int)newSize.Width, (int)newSize.Height);
            return target;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id) {

            if (id == null) {
                return BadRequest();
            }

            var blog = _articleRepo.Get(x => x.Id == id);
            _articleRepo.Delete(blog);
            return Json("Blog Id :" + blog.Id);
        }

        [AllowAnonymous]
        public IActionResult Detail(int id) {
            var blog = _articleRepo.Get(x => x.Id == id);
            return View(blog);
        }
    }
}