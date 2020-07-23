using BlogApp.Core.EntityFramework;
using BlogApp.DataAccess.Abstract;
using BlogApp.Entity.Concrete;

namespace BlogApp.DataAccess.Concrete.EntityFramework {
    public class EfCategoryDal : EfRepositoryBase<Category, BlogDbContext>, ICategoryDal {
    }
}
