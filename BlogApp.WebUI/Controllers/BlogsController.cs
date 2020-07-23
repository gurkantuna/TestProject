using BlogApp.DataAccess.Abstract;
using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using BlogApp.Core.Constants;

namespace BlogApp.WebUI.Controllers {

    [Authorize]
    public class BlogsController : Controller {
        private readonly IBlogDal _blogDal;
        private readonly ICategoryDal _categoryDal;
        private readonly ILogDal _logDal;

        public BlogsController(IBlogDal blogDal, ICategoryDal categoryDal, ILogDal logDal) {
            _blogDal = blogDal;
            _categoryDal = categoryDal;
            _logDal = logDal;
        }

        public ActionResult Index() {
            return View();
        }

        [Route("/Operations")]
        public IActionResult List() {

            Log log = new Log {
                Audit = "INFO",
                Date = DateTime.Now,
                Detail = "Home.Index()",
                User = User.Identity.IsAuthenticated ? User.Identity.Name : ConstStrings.Anonymous,
                Ip = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logDal.Add(log);

            var blogs = _blogDal.GetList();
            return View(blogs);
        }

        public IActionResult Create() {
            ViewBag.Categories = new SelectList(_categoryDal.GetList(), "Id", "Name");
            return View(new Blog());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, IFormFile file) {

            if (file?.Length > 5000000) {
                ModelState.AddModelError(ConstStrings.FileSize, ConstStrings.FileLimit);
            }

            if (file != null && !file.ContentType.Contains(ConstStrings.Image)) {
                ModelState.AddModelError(ConstStrings.WrongType, ConstStrings.ImageError);
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
                    blog.Body = ConstStrings.NonBodyInArticle;
                }

                blog.Date = DateTime.Now;
                _blogDal.Add(blog);
                return RedirectToAction("List", "Blogs");
            }
            ViewBag.Categories = new SelectList(_categoryDal.GetList(), "Id", "Name");
            return View(blog);
        }

        private async Task UploadImage(Blog blog, IFormFile file) {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img", file.FileName);
            FileInfo fileInfo = new FileInfo(file.FileName);
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
            var blog = _blogDal.Get(x => x.Id == id);
            ViewBag.Categories = new SelectList(_categoryDal.GetList(), "Id", "Name");
            return View(blog);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Blog blog, IFormFile file) {
            if (ModelState.IsValid) {
                blog.Date = DateTime.Now;
                if (file != null) {
                    await UploadImage(blog, file);
                }

                _blogDal.Update(blog);
                TempData["message"] = $"{blog.Title} updated";
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_categoryDal.GetList(), "Id", "Name");
            return View(blog);
        }

        // TODO : Benzer işler Business'a taşınmalı
        private async Task CopyToStreamImage(Blog blog, IFormFile file, string path) {
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

            var blog = _blogDal.Get(x => x.Id == id);
            _blogDal.Delete(blog);
            return Json("Blog Id :" + blog.Id);
        }

        [AllowAnonymous]
        public IActionResult Detail(int id) {
            var blog = _blogDal.Get(x => x.Id == id);
            return View(blog);
        }
    }
}