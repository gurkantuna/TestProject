using System;
using System.Collections;
using System.Linq;
using BlogApp.Core.Constants;
using BlogApp.DataAccess.EntitFrameworkCore.Abstract;
using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers {
    public class HomeController : Controller {

        public HomeController(IArticleRepo blogRepo, ILogRepo logRepo) {
            _articleRepo = blogRepo;
            _logRepo = logRepo;
        }

        private readonly IArticleRepo _articleRepo;
        private readonly ILogRepo _logRepo;

        public IActionResult Index() {

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

        [Route("/blog/{categoryId?}")]
        public IActionResult List(int? categoryId, string q) {

            IEnumerable articles;

            if (!string.IsNullOrWhiteSpace(q)) {
                articles = _articleRepo.GetList(
                    x => x.Title.Contains(q)
                      || x.Body.Contains(q)
                      || x.Description.Contains(q)
                      || x.Image.Contains(q));

                return View(articles);
            }

            if (categoryId.HasValue) {
                articles = _articleRepo.GetList(x => x.CategoryId == categoryId
                                        && x.IsApproved)
                                        .OrderByDescending(x => x.Date);
            }
            else {
                articles = _articleRepo.GetList(x => x.IsApproved)
                                        .OrderByDescending(x => x.Date);
            }

            return View(articles);
        }

        public IActionResult Details() {
            return View();
        }
    }
}