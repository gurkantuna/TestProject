using BlogApp.DataAccess.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.ViewComponents {
    public class CategoryViewComponent : ViewComponent {

        private readonly ICategoryDal _categoryDal;

        public CategoryViewComponent(ICategoryDal categoryDal) {
            _categoryDal = categoryDal;
        }

        public IViewComponentResult Invoke() {
            var categories = _categoryDal.GetList();
            ViewBag.SelectedCategory = RouteData?.Values["categoryId"];
            return View(categories);
        }
    }
}
