using BlogApp.DataAccess.EntitFrameworkCore.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.ViewComponents {
    public class CategoryViewComponent : ViewComponent {

        private readonly ICategoryRepo _categoryDal;

        public CategoryViewComponent(ICategoryRepo categoryDal) {
            _categoryDal = categoryDal;
        }

        public IViewComponentResult Invoke() {
            var categories = _categoryDal.GetList();
            ViewBag.SelectedCategory = RouteData?.Values["categoryId"];
            return View(categories);
        }
    }
}
