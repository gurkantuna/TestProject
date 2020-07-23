using BlogApp.Core.Constants;
using BlogApp.DataAccess.Abstract;
using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Linq;

namespace BlogApp.WebUI.Controllers {
    public class HomeController : Controller {

        private readonly IBlogDal _blogDal;
        private readonly ILogDal _logDal;

        public HomeController(IBlogDal blogDal, ILogDal logDal) {
            _blogDal = blogDal;
            _logDal = logDal;
        }

        public IActionResult Index() {

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

        [Route("/Blog/{categoryId?}")]
        public IActionResult List(int? categoryId, string q) {

            IEnumerable blogs;

            if (!string.IsNullOrWhiteSpace(q)) {
                blogs = _blogDal.GetList(
                    x => x.Title.Contains(q)
                      || x.Body.Contains(q)
                      || x.Description.Contains(q)
                      || x.Image.Contains(q));

                return View(blogs);
            }

            if (categoryId.HasValue) {
                blogs = _blogDal.GetList(x => x.CategoryId == categoryId
                                        && x.IsApproved)
                                        .OrderByDescending(x => x.Date);
            }
            else {
                blogs = _blogDal.GetList(x => x.IsApproved)
                                        .OrderByDescending(x => x.Date);
            }

            return View(blogs);
        }

        public IActionResult Details() {
            return View();
        }
    }
}